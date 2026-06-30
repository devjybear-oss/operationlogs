using System.Text.RegularExpressions;

namespace ChillPay.Core.Utils;

public static class StringUtil
{
    public static string MaskedConnectionString(string connectionString)
    {
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            return connectionString;
        }

        return Regex.Replace(connectionString, @"(Password|password)=.*?(;|$)", "Password=*****;", RegexOptions.None, TimeSpan.FromSeconds(5));
    }
}
