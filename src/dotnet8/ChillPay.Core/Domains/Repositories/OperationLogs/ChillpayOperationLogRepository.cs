using ChillPay.Core.Domains.Data;
using ChillPay.Core.Domains.Entities.OperationLogs;
using Microsoft.EntityFrameworkCore;

namespace ChillPay.Core.Domains.Repositories.OperationLogs;

public class ChillpayOperationLogRepository : GenericRepository<ChillpayOperationLogEntity>, IChillpayOperationLogRepository
{
    private readonly IOperationLogsDbContext _context;

    public ChillpayOperationLogRepository(IOperationLogsDbContext context)
        : base(context, context.ChillpayOperationLogs)
    {
        ArgumentNullException.ThrowIfNull(context, nameof(context));
        _context = context;
    }

    public Task<ChillpayOperationLogViewEntity> FindViewByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        return _context.ChillpayOperationLogsView
            .Where(m => m.Id == id)
            .AsNoTracking()
            .FirstOrDefaultAsync(cancellationToken);
    }

    public Task<long> CountAsync(
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
        CancellationToken cancellationToken = default)
    {
        var query = ApplySearch(
            _context.ChillpayOperationLogsView.AsNoTracking(),
            moduleTypes,
            menuTypes,
            logTypes,
            addedDateFrom,
            addedDateTo,
            searchText,
            dataId,
            merchantIds,
            refTypes,
            refId);

        return query.LongCountAsync(cancellationToken);
    }

    public Task<List<ChillpayOperationLogViewEntity>> SearchAsync(
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
        CancellationToken cancellationToken = default)
    {
        var query = ApplySearch(
            _context.ChillpayOperationLogsView.AsNoTracking(),
            moduleTypes,
            menuTypes,
            logTypes,
            addedDateFrom,
            addedDateTo,
            searchText,
            dataId,
            merchantIds,
            refTypes,
            refId);

        query = ApplyOrder(query, orderBy, orderDir);
        return query.Skip(start).Take(pageSize).ToListAsync(cancellationToken);
    }

    public override async Task<long> AddAsync(ChillpayOperationLogEntity entity, CancellationToken cancellationToken = default)
    {
        if (entity.AddedDate == default)
        {
            entity.AddedDate = DateTime.Now;
        }

        await _context.ChillpayOperationLogs.AddAsync(entity, cancellationToken);
        return entity.Id;
    }

    private static IQueryable<ChillpayOperationLogViewEntity> ApplySearch(
        IQueryable<ChillpayOperationLogViewEntity> query,
        int[] moduleTypes,
        int[] menuTypes,
        int[] logTypes,
        DateTime? addedDateFrom,
        DateTime? addedDateTo,
        string searchText,
        long? dataId,
        long[] merchantIds,
        int[] refTypes,
        long? refId)
    {
        if (moduleTypes != null && moduleTypes.Length > 0)
        {
            query = query.Where(m => moduleTypes.Contains(m.ModuleType));
        }

        if (menuTypes != null && menuTypes.Length > 0)
        {
            query = query.Where(m => menuTypes.Contains(m.MenuType));
        }

        if (logTypes != null && logTypes.Length > 0)
        {
            query = query.Where(m => logTypes.Contains(m.LogType));
        }

        if (addedDateFrom.HasValue)
        {
            query = query.Where(m => m.AddedDate >= addedDateFrom.Value);
        }

        if (addedDateTo.HasValue)
        {
            query = query.Where(m => m.AddedDate <= addedDateTo.Value);
        }

        if (dataId.HasValue && dataId.Value > 0)
        {
            query = query.Where(m => m.DataId == dataId.Value);
        }

        if (merchantIds != null && merchantIds.Length > 0)
        {
            query = query.Where(m => m.MerchantId.HasValue && merchantIds.Contains(m.MerchantId.Value));
        }

        if (refTypes != null && refTypes.Length > 0 && refId.HasValue && refId.Value > 0)
        {
            query = query.Where(m =>
                (m.RefType.HasValue && refTypes.Contains(m.RefType.Value) && m.RefId == refId.Value)
                || (m.Ref2Type.HasValue && refTypes.Contains(m.Ref2Type.Value) && m.Ref2Id == refId.Value));
        }

        if (!string.IsNullOrWhiteSpace(searchText))
        {
            query = query.Where(m =>
                (m.Message != null && m.Message.Contains(searchText))
                || (m.OldValue != null && m.OldValue.Contains(searchText))
                || (m.NewValue != null && m.NewValue.Contains(searchText))
                || (m.RequestByName != null && m.RequestByName.Contains(searchText))
                || (m.MerchantCode != null && m.MerchantCode.Contains(searchText))
                || (m.ShortName != null && m.ShortName.Contains(searchText))
                || (m.CompanyName != null && m.CompanyName.Contains(searchText))
                || (m.ShortNameEN != null && m.ShortNameEN.Contains(searchText)));
        }

        return query;
    }

    private static IQueryable<ChillpayOperationLogViewEntity> ApplyOrder(
        IQueryable<ChillpayOperationLogViewEntity> query,
        string orderBy,
        string orderDir)
    {
        var isAsc = string.Equals(orderDir, "asc", StringComparison.OrdinalIgnoreCase);
        var column = (orderBy ?? string.Empty).ToLowerInvariant();

        return column switch
        {
            "moduletype" => isAsc ? query.OrderBy(m => m.ModuleType) : query.OrderByDescending(m => m.ModuleType),
            "menutype" => isAsc ? query.OrderBy(m => m.MenuType) : query.OrderByDescending(m => m.MenuType),
            "logtype" => isAsc ? query.OrderBy(m => m.LogType) : query.OrderByDescending(m => m.LogType),
            "message" => isAsc ? query.OrderBy(m => m.Message) : query.OrderByDescending(m => m.Message),
            "addeddate" => isAsc ? query.OrderBy(m => m.AddedDate) : query.OrderByDescending(m => m.AddedDate),
            "requestsystem" => isAsc ? query.OrderBy(m => m.RequestSystem) : query.OrderByDescending(m => m.RequestSystem),
            "requestby" => isAsc ? query.OrderBy(m => m.RequestBy) : query.OrderByDescending(m => m.RequestBy),
            "dataid" => isAsc ? query.OrderBy(m => m.DataId) : query.OrderByDescending(m => m.DataId),
            "refid" => isAsc ? query.OrderBy(m => m.RefId) : query.OrderByDescending(m => m.RefId),
            "merchantid" => isAsc ? query.OrderBy(m => m.MerchantId) : query.OrderByDescending(m => m.MerchantId),
            "merchantcode" => isAsc ? query.OrderBy(m => m.MerchantCode) : query.OrderByDescending(m => m.MerchantCode),
            "companyname" => isAsc ? query.OrderBy(m => m.CompanyName) : query.OrderByDescending(m => m.CompanyName),
            _ => isAsc ? query.OrderBy(m => m.Id) : query.OrderByDescending(m => m.Id),
        };
    }
}
