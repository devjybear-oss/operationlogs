using ChillPay.Core.Domains.Entities.OperationLogs;

namespace ChillPay.Core.Models.OperationLogs;

public class ChillpayOperationLogSearchResult
{
    public long TotalRecord { get; set; }
    public List<ChillpayOperationLogViewEntity> Items { get; set; }
}
