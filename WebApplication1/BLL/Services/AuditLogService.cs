using Messages;
using Microsoft.Extensions.Options;
using Models.Dto.Common;
using WebApplication1.BLL.Models;
using WebApplication1.Config;
using WebApplication1.DAL;
using WebApplication1.DAL.Interfaces;
using WebApplication1.DAL.Models;

namespace WebApplication1.BLL.Services;

public class AuditLogService(UnitOfWork unitOfWork, IAuditLogOrderRepository auditLogOrderRepository){
    public async Task<AuditLogOrderUnit[]> BatchInsert(AuditLogOrderUnit[] logUnits, CancellationToken token)
    {
        var now = DateTimeOffset.UtcNow;
        await using var transaction = await unitOfWork.BeginTransactionAsync(token);

        try
        {
            var dalModels = logUnits.Select(x => new V1AuditLogOrderDal
            {
                OrderId = x.OrderId,
                OrderItemId = x.OrderItemId,
                CustomerId = x.CustomerId,
                OrderStatus = x.OrderStatus,
                CreatedAt = now,
                UpdatedAt = now
            }).ToArray();

            var insertedLogs = await auditLogOrderRepository.BulkInsert(dalModels, token);

            await transaction.CommitAsync(token);
            
            var result = insertedLogs.Select(x => new AuditLogOrderUnit
            {
                OrderId = x.OrderId,
                OrderItemId = x.OrderItemId,
                CustomerId = x.CustomerId,
                OrderStatus = x.OrderStatus,
                CreatedAt = x.CreatedAt,
                UpdatedAt = x.UpdatedAt
            }).ToArray();

            return result;
        }
        catch
        {
            await transaction.RollbackAsync(token);
            throw;
        }
    }
}