using ChillPay.Core.Domains.Entities.OperationLogs;
using Microsoft.EntityFrameworkCore;

namespace ChillPay.Core.Domains.Data;

public interface IOperationLogsDbContext : IDbContext
{
    DbSet<ChillpayOperationLogEntity> ChillpayOperationLogs { get; set; }
    DbSet<ChillpayOperationLogViewEntity> ChillpayOperationLogsView { get; set; }
}
