using ChillPay.Core.Domains.Data;
using ChillPay.Core.Models.Generics;
using ChillPay.Core.Utils;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ChillPay.Core.Extensions;

public static class ServiceCollectionExtensions
{
    private const string SqlDbConnectionKey = "SqlDbConnection";
    private const string DbApplicationNameKey = "ApplicationName";

    public static IServiceCollection AddSqlServerDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptions();
        services.Configure<DatabaseSetting>(configuration.GetSection("DatabaseSettings"));

        var connectionString = GetSqlConnectionString(configuration);
        var maskedConnectionString = StringUtil.MaskedConnectionString(connectionString);
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"Connect to db server >> {maskedConnectionString}");
        Console.ResetColor();

        using var connection = new SqlConnection(connectionString);
        connection.Open();
        using var command = connection.CreateCommand();
        command.CommandText = "SELECT 1;";
        command.ExecuteScalar();

        services.AddDbContext<IOperationLogsDbContext, OperationLogsDbContext>(options =>
        {
            options.UseSqlServer(connectionString, opt =>
            {
                opt.CommandTimeout(60);
            });
            options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
        });

        return services;
    }

    private static string GetSqlConnectionString(IConfiguration configuration)
    {
        string connectionString = Environment.GetEnvironmentVariable(configuration.GetConnectionString(SqlDbConnectionKey));
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            connectionString = configuration.GetConnectionString(SqlDbConnectionKey) + string.Empty;
        }

        string appName = configuration.GetConnectionString(DbApplicationNameKey);
        string trustServerCertificate = GetSqlTrustServerCertificate(configuration);

        if (!connectionString.Contains("TrustServerCertificate"))
        {
            connectionString = connectionString + ";TrustServerCertificate=" + trustServerCertificate;
        }

        if (!connectionString.Contains("application name="))
        {
            connectionString = connectionString + ";application name=" + appName;
        }

        return connectionString;
    }

    private static string GetSqlTrustServerCertificate(IConfiguration configuration)
    {
        string trustServerCertificate = configuration.GetConnectionString("TrustServerCertificate");
        if (string.IsNullOrEmpty(trustServerCertificate))
        {
            trustServerCertificate = "True";
        }

        return trustServerCertificate;
    }
}
