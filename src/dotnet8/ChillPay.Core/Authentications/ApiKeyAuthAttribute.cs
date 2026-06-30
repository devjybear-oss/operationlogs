namespace ChillPay.Core.Authentications;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class ApiKeyAuthAttribute : Microsoft.AspNetCore.Mvc.ServiceFilterAttribute
{
    public ApiKeyAuthAttribute() : base(typeof(ApiKeyAuthFilter))
    {
    }
}
