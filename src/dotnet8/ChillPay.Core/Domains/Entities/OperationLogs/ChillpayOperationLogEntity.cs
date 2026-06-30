using System.ComponentModel.DataAnnotations.Schema;
using ChillPay.Core.Domains.Entities;

namespace ChillPay.Core.Domains.Entities.OperationLogs;

[Table("ChillpayOperationLogs")]
public class ChillpayOperationLogEntity : BaseEntity
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
    public DateTime AddedDate { get; set; }
}
