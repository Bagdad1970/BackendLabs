using Microsoft.AspNetCore.Mvc;
using Models.Dto.Common;
using Models.Dto.V1.Requests;
using Models.Dto.V1.Responses;
using WebApplication1.BLL.Services;
using WebApplication1.Validators;


namespace WebApplication1.Controllers.V1;


[Route("api/v1/audit/log-order")]
public class AuditLogOrderController(AuditLogService auditLogService, ValidatorFactory validatorFactory) : ControllerBase
{
    [HttpPost("batch-create")]
    public async Task<ActionResult<V1AuditLogOrderResponse>> V1BatchCreate([FromBody] V1CreateAuditLogRequest request, CancellationToken token)
    {
        var validationResult = await validatorFactory.GetValidator<V1CreateAuditLogRequest>().ValidateAsync(request, token);
        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.ToDictionary());
        }
        
        var auditLogOrderUnits = request.Orders.Select(x => new AuditLogOrderUnit
        {
            OrderId = x.OrderId,
            OrderItemId = x.OrderItemId,
            CustomerId = x.CustomerId,
            OrderStatus = x.OrderStatus
        }).ToArray();
        
        var res = await auditLogService.BatchInsert(auditLogOrderUnits, token);

        return Ok(new V1AuditLogOrderResponse
        {
            Orders = Map(res)
        });
    }
    
    private AuditLogOrderUnit[] Map(AuditLogOrderUnit[] logs)
    {
        return logs.Select(x => new AuditLogOrderUnit
        {
            OrderId = x.OrderId,
            OrderItemId = x.OrderItemId,
            CustomerId = x.CustomerId,
            OrderStatus = x.OrderStatus,
            CreatedAt = x.CreatedAt,
            UpdatedAt = x.UpdatedAt,
        }).ToArray();
    }
}