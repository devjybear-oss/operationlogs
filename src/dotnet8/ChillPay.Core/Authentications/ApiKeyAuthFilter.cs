using ChillPay.Core.Extensions;
using ChillPay.Core.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;

namespace ChillPay.Core.Authentications;

public class ApiKeyAuthFilter(IConfiguration configuration) : IAuthorizationFilter
{
    private const string ApiKeyHeaderName = "WebAppSetting:CHILLPAY_HEADER_KEY";
    private const string ApiKeySectionName = "WebAppSetting:CHILLPAY_TOKEN";

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var apiHeaderName = configuration.GetValue<string>(ApiKeyHeaderName);
        var unauthorizedResult = new UnauthorizedObjectResult(ApiResponseModel.Failed($"API Key is missing ({apiHeaderName})"));

        if (!context.HttpContext.Request.Headers.TryGetValue(apiHeaderName, out var apiKeyHeader))
        {
            context.Result = unauthorizedResult;
            return;
        }

        if (string.IsNullOrWhiteSpace(apiKeyHeader))
        {
            context.Result = unauthorizedResult;
            return;
        }

        string apiKeyConfig = configuration.GetChillPayToken(ApiKeySectionName);
        if (!apiKeyHeader.Equals(apiKeyConfig))
        {
            context.Result = new UnauthorizedObjectResult(ApiResponseModel.Failed("Invalid API Key"));
        }
    }
}
