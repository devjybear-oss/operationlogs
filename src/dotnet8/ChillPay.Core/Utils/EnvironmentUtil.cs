using Microsoft.Extensions.Configuration;

namespace ChillPay.Core.Utils;

public static class EnvironmentUtil
{
    public static string GetVariable(string envName)
    {
        if (string.IsNullOrEmpty(envName))
        {
            return default;
        }

        var envValue = Environment.GetEnvironmentVariable(envName);
        if (!string.IsNullOrEmpty(envValue))
        {
            return envValue;
        }

        return envName;
    }

    public static string GetChillPayToken(IConfiguration configuration, string keyName = "WebAppSetting:CHILLPAY_TOKEN")
    {
        return GetVariable(configuration.GetValue<string>(keyName));
    }
}
