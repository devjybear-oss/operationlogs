namespace ChillPay.Core.Domains.Entities.OperationLogs;

public class ChillpayOperationLogViewEntity
{
    public long Id { get; set; }
    public int ModuleType { get; set; }
    public int MenuType { get; set; }
    public int LogType { get; set; }
    public string Message { get; set; }
    public string OldValue { get; set; }
    public string NewValue { get; set; }
    public long? DataId { get; set; }
    public int? RefType { get; set; }
    public long? RefId { get; set; }
    public int? Ref2Type { get; set; }
    public long? Ref2Id { get; set; }
    public string RequestSystem { get; set; }
    public long RequestBy { get; set; }
    public string RequestByName { get; set; }
    public DateTime AddedDate { get; set; }
    public string AddedDateText { get; set; }
    public string ModuleTypeText { get; set; }
    public string MenuTypeText { get; set; }
    public string LogTypeText { get; set; }
    public string RefTypeText { get; set; }
    public string Ref2TypeText { get; set; }

    public long? MerchantId { get; set; }
    public string MerchantCode { get; set; }
    public string ShortName { get; set; }
    public string CompanyName { get; set; }
    public string ShortNameEN { get; set; }
}
