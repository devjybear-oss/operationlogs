using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace ChillPay.OperationLogsApi.Controllers.V1;

public abstract class BaseController : ControllerBase
{
    protected readonly CancellationToken CancellationToken;

    private static readonly JsonSerializerOptions JsonFormatOptions = new()
    {
        DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.Never,
        PropertyNameCaseInsensitive = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
    };

    private static readonly JsonSerializerOptions JsonFormatIgnoreNullOptions = new()
    {
        DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull,
        PropertyNameCaseInsensitive = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
    };

    protected BaseController()
    {
        CancellationToken = HttpContext?.RequestAborted ?? CancellationToken.None;
    }

    protected static string JsonSerializeObject(object value, bool ignoreNull = true)
    {
        if (value is null)
        {
            return string.Empty;
        }

        if (ignoreNull)
        {
            return JsonSerializer.Serialize(value, JsonFormatIgnoreNullOptions);
        }

        return JsonSerializer.Serialize(value, JsonFormatOptions);
    }
}
