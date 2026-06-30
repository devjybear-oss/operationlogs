using ChillPay.Core.Domains.Entities.OperationLogs;
using ChillPay.Core.Models.OperationLogs;

namespace ChillPay.Core.Services.OperationLogs;

public interface IChillpayOperationLogService
{
    Task<ChillpayOperationLogViewEntity> FindByIdAsync(long id, string requestSystem, long requestBy, CancellationToken cancellationToken = default);

    Task<ChillpayOperationLogSearchResult> SearchAsync(SearchChillpayOperationLogModel model, CancellationToken cancellationToken = default);

    Task<ChillpayOperationLogSearchResult> SearchByMenuAsync(SearchChillpayOperationLogByMenuModel model, CancellationToken cancellationToken = default);

    Task<long> AddAsync(AddChillpayOperationLogModel model, CancellationToken cancellationToken = default);
}
