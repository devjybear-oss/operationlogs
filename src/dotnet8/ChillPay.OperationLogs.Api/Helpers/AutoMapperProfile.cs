using ChillPay.Core.Domains.Entities.OperationLogs;
using ChillPay.Core.Models.OperationLogs;

namespace ChillPay.OperationLogsApi.Helpers;

public class AutoMapperProfile : AutoMapper.Profile
{
    public AutoMapperProfile()
    {
        CreateMap<AddChillpayOperationLogModel, ChillpayOperationLogEntity>();
    }
}
