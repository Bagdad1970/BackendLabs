using WebApplication1.DAL.Models;

namespace WebApplication1.DAL.Interfaces;

public interface IAuditLogOrderRepository
{
    Task<V1AuditLogOrderDal[]> BulkInsert(V1AuditLogOrderDal[] model, CancellationToken token);
    
}