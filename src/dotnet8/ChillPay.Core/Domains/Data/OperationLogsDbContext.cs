using ChillPay.Core.Domains.Entities.OperationLogs;
using ChillPay.Core.Models.Generics;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace ChillPay.Core.Domains.Data;

public class OperationLogsDbContext(DbContextOptions<OperationLogsDbContext> options, IOptions<DatabaseSetting> configs)
    : DbContext(options), IOperationLogsDbContext
{
    private readonly string _schema = string.IsNullOrWhiteSpace(configs?.Value.SqlDbSchema) ? "dbo" : configs.Value.SqlDbSchema;

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<ChillpayOperationLogEntity>(b =>
        {
            b.HasKey(m => m.Id);
            b.ToTable("ChillpayOperationLogs", Schema);
        });

        builder.Entity<ChillpayOperationLogViewEntity>(b =>
        {
            b.HasKey(m => m.Id);
            b.ToView("VW_ChillpayOperationLogs");
        });
    }

    public string Schema => _schema;

    public virtual DbSet<ChillpayOperationLogEntity> ChillpayOperationLogs { get; set; }
    public virtual DbSet<ChillpayOperationLogViewEntity> ChillpayOperationLogsView { get; set; }
}
