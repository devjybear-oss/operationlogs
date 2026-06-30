using ChillPay.Core.Constants.OperationLogs;

namespace ChillPay.Core.Models.OperationLogs;

public class AddChillpayOperationLogModel
{
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
    public long? MerchantId { get; set; }
    public string RequestSystem { get; set; }
    public long RequestBy { get; set; }
    public string RequestByName { get; set; }

    public bool IsValid()
    {
        if (ModuleType <= 0 || MenuType <= 0 || LogType <= 0 || RequestBy <= 0)
        {
            return false;
        }

        if (!ChillpayOperationLogRegistry.CheckValidRequestSystem(RequestSystem))
        {
            return false;
        }

        return ChillpayOperationLogRegistry.IsValidAddRequest(ModuleType, MenuType, LogType, RefType, Ref2Type);
    }
}
