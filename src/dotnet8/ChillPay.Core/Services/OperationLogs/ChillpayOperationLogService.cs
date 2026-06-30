using AutoMapper;
using ChillPay.Core.Constants;
using ChillPay.Core.Constants.OperationLogs;
using ChillPay.Core.Domains.Entities.OperationLogs;
using ChillPay.Core.Domains.Repositories.OperationLogs;
using ChillPay.Core.Exceptions;
using ChillPay.Core.Models.OperationLogs;
using Microsoft.Extensions.Logging;

namespace ChillPay.Core.Services.OperationLogs;

public class ChillpayOperationLogService(
    ILogger<ChillpayOperationLogService> logger,
    IChillpayOperationLogRepository repository,
    IMapper mapper) : IChillpayOperationLogService
{
    public async Task<ChillpayOperationLogViewEntity> FindByIdAsync(
        long id,
        string requestSystem,
        long requestBy,
        CancellationToken cancellationToken = default)
    {
        ValidateRequestContext(id, requestSystem, requestBy);

        var item = await repository.FindViewByIdAsync(id, cancellationToken);
        if (item == null)
        {
            throw new NotFoundException(nameof(ChillpayOperationLogViewEntity), id);
        }

        return item;
    }

    public async Task<ChillpayOperationLogSearchResult> SearchAsync(
        SearchChillpayOperationLogModel model,
        CancellationToken cancellationToken = default)
    {
        if (model == null || !model.IsValid())
        {
            throw new InvalidParameterException("Invalid Parameter");
        }

        DateTime? addedDateFrom = model.AddedDateFromTick.HasValue ? new DateTime(model.AddedDateFromTick.Value) : null;
        DateTime? addedDateTo = model.AddedDateToTick.HasValue ? new DateTime(model.AddedDateToTick.Value) : null;

        var totalRecord = await repository.CountAsync(
            model.ModuleType,
            model.MenuType,
            model.LogType,
            addedDateFrom,
            addedDateTo,
            model.SearchText,
            model.DataId,
            model.MerchantId,
            model.RefType,
            model.RefId,
            cancellationToken);

        var items = await repository.SearchAsync(
            model.ModuleType,
            model.MenuType,
            model.LogType,
            addedDateFrom,
            addedDateTo,
            model.SearchText,
            model.DataId,
            model.MerchantId,
            model.RefType,
            model.RefId,
            model.OrderBy,
            model.OrderDir,
            model.Start,
            model.PageSize,
            cancellationToken);

        return new ChillpayOperationLogSearchResult
        {
            TotalRecord = totalRecord,
            Items = items,
        };
    }

    public async Task<ChillpayOperationLogSearchResult> SearchByMenuAsync(
        SearchChillpayOperationLogByMenuModel model,
        CancellationToken cancellationToken = default)
    {
        if (model == null || !model.IsValid())
        {
            throw new InvalidParameterException("Invalid Parameter");
        }

        var moduleTypes = new[] { model.ModuleType };
        var menuTypes = new[] { model.MenuType };

        var totalRecord = await repository.CountAsync(
            moduleTypes,
            menuTypes,
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            cancellationToken);

        var items = await repository.SearchAsync(
            moduleTypes,
            menuTypes,
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            model.OrderBy,
            model.OrderDir,
            model.Start,
            model.PageSize,
            cancellationToken);

        return new ChillpayOperationLogSearchResult
        {
            TotalRecord = totalRecord,
            Items = items,
        };
    }

    public async Task<long> AddAsync(AddChillpayOperationLogModel model, CancellationToken cancellationToken = default)
    {
        if (model == null)
        {
            throw new InvalidParameterException("Invalid Parameter");
        }

        if (!model.IsValid())
        {
            throw new InvalidParameterException("Invalid Parameter");
        }

        try
        {
            var entity = mapper.Map<ChillpayOperationLogEntity>(model);
            entity.MerchantId = ChillpayOperationLogRegistry.ResolveMerchantId(
                entity.ModuleType,
                entity.MenuType,
                entity.RefType,
                entity.RefId,
                entity.Ref2Type,
                entity.Ref2Id,
                entity.DataId,
                model.MerchantId);
            _ = await repository.AddAsync(entity, cancellationToken);
            await repository.SaveChangesAsync(cancellationToken);
            return entity.Id;
        }
        catch (InvalidParameterException)
        {
            throw;
        }
        catch (Exception ex)
        {
            logger.LogError(AppLogEvents.Error, ex, ex.Message);
            throw;
        }
    }

    private static void ValidateRequestContext(long id, string requestSystem, long requestBy)
    {
        if (id <= 0 || requestBy <= 0)
        {
            throw new InvalidParameterException("Invalid Parameter");
        }

        if (!ChillpayOperationLogRegistry.CheckValidRequestSystem(requestSystem))
        {
            throw new InvalidParameterException("Invalid Parameter");
        }
    }
}
