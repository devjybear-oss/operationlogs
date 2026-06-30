using ChillPay.Core.Constants.OperationLogs;

namespace ChillPay.Core.Models.OperationLogs;

public class SearchChillpayOperationLogModel
{
    public int Draw { get; set; }
    public string OrderBy { get; set; }
    public string OrderDir { get; set; }
    public int Start { get; set; }
    public int PageSize { get; set; }

    public int[] ModuleType { get; set; }
    public int[] MenuType { get; set; }
    public int[] LogType { get; set; }
    public long? AddedDateFromTick { get; set; }
    public long? AddedDateToTick { get; set; }
    public string SearchText { get; set; }
    public long? DataId { get; set; }
    public long[] MerchantId { get; set; }
    public int[] RefType { get; set; }
    public long? RefId { get; set; }

    public string RequestSystem { get; set; }
    public long RequestBy { get; set; }

    public bool IsValid()
    {
        if (Start < 0 || PageSize <= 0 || RequestBy <= 0)
        {
            return false;
        }

        if (!ChillpayOperationLogRegistry.CheckValidRequestSystem(RequestSystem))
        {
            return false;
        }

        if (ModuleType != null)
        {
            foreach (var module in ModuleType)
            {
                if (!ChillpayOperationLogRegistry.IsValidModuleType(module))
                {
                    return false;
                }
            }
        }

        if (MenuType != null)
        {
            foreach (var menu in MenuType)
            {
                if (!ChillpayOperationLogRegistry.IsValidMenuType(menu))
                {
                    return false;
                }
            }
        }

        return true;
    }
}

public class SearchChillpayOperationLogByMenuModel
{
    public int Draw { get; set; }
    public string OrderBy { get; set; }
    public string OrderDir { get; set; }
    public int Start { get; set; }
    public int PageSize { get; set; }

    public int ModuleType { get; set; }
    public int MenuType { get; set; }

    public string RequestSystem { get; set; }
    public long RequestBy { get; set; }

    public bool IsValid()
    {
        if (ModuleType <= 0 || MenuType <= 0 || Start < 0 || PageSize <= 0 || RequestBy <= 0)
        {
            return false;
        }

        if (!ChillpayOperationLogRegistry.CheckValidRequestSystem(RequestSystem))
        {
            return false;
        }

        return ChillpayOperationLogRegistry.IsValidModuleType(ModuleType)
            && ChillpayOperationLogRegistry.IsValidMenuType(MenuType)
            && ChillpayOperationLogRegistry.IsMenuTypeInModule(MenuType, ModuleType);
    }
}
