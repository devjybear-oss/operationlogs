namespace ChillPay.Core.Models.Generics;

public record BaseAppSetting
{
    public string EnvServerMode { get; set; }
    public string CHILLPAY_HEADER_KEY { get; set; }
    public string CHILLPAY_TOKEN { get; set; }
}
