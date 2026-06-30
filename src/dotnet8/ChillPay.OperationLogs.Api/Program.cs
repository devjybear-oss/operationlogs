using ChillPay.Core.Authentications;
using ChillPay.Core.Models;
using ChillPay.OperationLogsApi.Extensions;
using ChillPay.OperationLogsApi.Helpers;
using Serilog;

Log.Logger = new LoggerConfiguration().WriteTo.Console()
    .MinimumLevel.Information()
    .CreateLogger();

try
{
    Log.Information("Starting ChillPay Operation Logs API");

    const string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

    var builder = WebApplication.CreateBuilder(args);

    builder.Host.UseSerilog((context, logging) =>
    {
        logging.ReadFrom.Configuration(context.Configuration);
        logging.WriteTo.Console();
    });

    builder.Host.ConfigureServices((context, services) =>
    {
        services.Configure<HostOptions>(option =>
        {
            option.ShutdownTimeout = TimeSpan.FromSeconds(30);
        });
    });

    builder.Services.AddHttpContextAccessor();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen(option =>
    {
        option.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
    });
    builder.Services.AddControllers(opt => { opt.AllowEmptyInputInBodyModelBinding = true; })
        .AddJsonOptions(opt =>
        {
            opt.JsonSerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.Never;
        });
    builder.Services.AddAutoMapper(typeof(AutoMapperProfile));
    builder.Services.Configure<WebAppSetting>(builder.Configuration.GetSection(nameof(WebAppSetting)));

    builder.Services.AddChillPayDatabases(builder.Configuration);
    builder.Services.AddChillPayServices();
    builder.Services.AddScoped<ApiKeyAuthFilter>();
    builder.Services.AddHealthChecks();
    builder.Services.AddCors(options =>
    {
        options.AddPolicy(MyAllowSpecificOrigins, config =>
        {
            config.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader();
        });
    });

    var app = builder.Build();

    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseAuthorization();
    app.UseCors(MyAllowSpecificOrigins);
    app.MapControllers();
    app.MapHealthChecks("/healthz");

    app.Run();
}
catch (Exception ex)
{
    Log.Error(ex, "Application terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}
