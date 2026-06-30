using ChillPay.Core.Authentications;
using ChillPay.Core.Domains.Repositories.OperationLogs;
using ChillPay.Core.Extensions;
using ChillPay.Core.Services.OperationLogs;

namespace ChillPay.OperationLogsApi.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddChillPayDatabases(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSqlServerDatabase(configuration);
        return services;
    }

    public static IServiceCollection AddChillPayServices(this IServiceCollection services)
    {
        services.AddScoped<IChillpayOperationLogRepository, ChillpayOperationLogRepository>();
        services.AddScoped<IChillpayOperationLogService, ChillpayOperationLogService>();
        return services;
    }
}
