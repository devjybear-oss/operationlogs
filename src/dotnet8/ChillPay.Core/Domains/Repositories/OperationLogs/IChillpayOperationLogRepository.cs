using ChillPay.Core.Domains.Entities.OperationLogs;
using ChillPay.Core.Domains.Repositories;

namespace ChillPay.Core.Domains.Repositories.OperationLogs;

public interface IChillpayOperationLogRepository : IGenericRepository<ChillpayOperationLogEntity>
{
    Task<ChillpayOperationLogViewEntity> FindViewByIdAsync(long id, CancellationToken cancellationToken = default);

    Task<long> CountAsync(
        int[] moduleTypes,
        int[] menuTypes,
        int[] logTypes,
        DateTime? addedDateFrom,
        DateTime? addedDateTo,
        string searchText,
        long? dataId,
        long[] merchantIds,
        int[] refTypes,
        long? refId,
        CancellationToken cancellationToken = default);

    Task<List<ChillpayOperationLogViewEntity>> SearchAsync(
        int[] moduleTypes,
        int[] menuTypes,
        int[] logTypes,
        DateTime? addedDateFrom,
        DateTime? addedDateTo,
        string searchText,
        long? dataId,
        long[] merchantIds,
        int[] refTypes,
        long? refId,
        string orderBy,
        string orderDir,
        int start,
        int pageSize,
        CancellationToken cancellationToken = default);
}
