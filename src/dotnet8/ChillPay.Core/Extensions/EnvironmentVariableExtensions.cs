using ChillPay.Core.Utils;
using Microsoft.Extensions.Configuration;

namespace ChillPay.Core.Extensions;

public static class EnvironmentVariableExtensions
{
    public static string GetChillPayToken(this IConfiguration configuration, string keyName = "WebAppSetting:CHILLPAY_TOKEN")
    {
        return EnvironmentUtil.GetChillPayToken(configuration, keyName);
    }
}
