using ChillPay.Core.Authentications;
using ChillPay.Core.Constants;
using ChillPay.Core.Domains.Entities.OperationLogs;
using ChillPay.Core.Exceptions;
using ChillPay.Core.Models;
using ChillPay.Core.Models.OperationLogs;
using ChillPay.Core.Services.OperationLogs;
using ChillPay.OperationLogsApi.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;

namespace ChillPay.OperationLogsApi.Controllers.V1;

[ApiController]
[ApiKeyAuth]
[Route("api/v1/chillpayoperationlogs")]
[Produces(MediaTypeNames.Application.Json)]
public class ChillpayOperationLogController(
    ILoggerFactory loggerFactory,
    IChillpayOperationLogService chillpayOperationLogService) : BaseController
{
    private readonly ILogger<ChillpayOperationLogController> _logger = loggerFactory.CreateLogger<ChillpayOperationLogController>();

    [HttpGet]
    public IActionResult Get()
    {
        return BadRequest(ApiResponseModel.Failed());
    }

    [HttpGet("{id:long}/{requestSystem}/{requestBy:long}")]
    public async Task<IActionResult> FindById(long id, string requestSystem, long requestBy)
    {
        _logger.LogInformation(AppLogEvents.Details, "[FindById]Id: {Id}", id);

        try
        {
            var item = await chillpayOperationLogService.FindByIdAsync(id, requestSystem, requestBy, CancellationToken);
            var response = DataTableResponseMessageModel<ChillpayOperationLogViewEntity>.Success(item);
            response.TotalRecord = 1;
            response.FilteredRecord = 1;
            response.Start = 0;
            response.PageSize = 1;
            return Ok(response);
        }
        catch (InvalidParameterException ex)
        {
            _logger.LogError(AppLogEvents.Error, "[FindById]{Error}", ex.Message);
            return BadRequest(DataTableResponseMessageModel<ChillpayOperationLogViewEntity>.Failed(ex.Message));
        }
        catch (NotFoundException ex)
        {
            _logger.LogError(AppLogEvents.Error, "[FindById]{Error}", ex.Message);
            return NotFound(DataTableResponseMessageModel<ChillpayOperationLogViewEntity>.Failed(ex.Message));
        }
        catch (Exception ex)
        {
            _logger.LogError(AppLogEvents.Error, ex, ex.Message);
        }

        return BadRequest(DataTableResponseMessageModel<ChillpayOperationLogViewEntity>.Failed("System Error"));
    }

    [HttpPost("search")]
    public async Task<IActionResult> Search(SearchChillpayOperationLogModel model)
    {
        _logger.LogInformation(AppLogEvents.Details, "[Search]Model: {JsonModel}", JsonSerializeObject(model, false));

        try
        {
            var result = await chillpayOperationLogService.SearchAsync(model, CancellationToken);
            var response = DataTableResponseMessageModel<ChillpayOperationLogViewEntity[]>.Success(result.Items.ToArray());
            response.Draw = model.Draw;
            response.Start = model.Start;
            response.PageSize = model.PageSize;
            response.TotalRecord = result.TotalRecord;
            response.FilteredRecord = result.TotalRecord;
            return Ok(response);
        }
        catch (InvalidParameterException ex)
        {
            _logger.LogError(AppLogEvents.Error, "[Search]{Error}", ex.Message);
            return BadRequest(DataTableResponseMessageModel<ChillpayOperationLogViewEntity[]>.Failed(ex.Message));
        }
        catch (Exception ex)
        {
            _logger.LogError(AppLogEvents.Error, ex, ex.Message);
        }

        return BadRequest(DataTableResponseMessageModel<ChillpayOperationLogViewEntity[]>.Failed("System Error"));
    }

    [HttpPost("search/menu")]
    public async Task<IActionResult> SearchByMenu(SearchChillpayOperationLogByMenuModel model)
    {
        _logger.LogInformation(AppLogEvents.Details, "[SearchByMenu]Model: {JsonModel}", JsonSerializeObject(model, false));

        try
        {
            var result = await chillpayOperationLogService.SearchByMenuAsync(model, CancellationToken);
            var response = DataTableResponseMessageModel<ChillpayOperationLogViewEntity[]>.Success(result.Items.ToArray());
            response.Draw = model.Draw;
            response.Start = model.Start;
            response.PageSize = model.PageSize;
            response.TotalRecord = result.TotalRecord;
            response.FilteredRecord = result.TotalRecord;
            return Ok(response);
        }
        catch (InvalidParameterException ex)
        {
            _logger.LogError(AppLogEvents.Error, "[SearchByMenu]{Error}", ex.Message);
            return BadRequest(DataTableResponseMessageModel<ChillpayOperationLogViewEntity[]>.Failed(ex.Message));
        }
        catch (Exception ex)
        {
            _logger.LogError(AppLogEvents.Error, ex, ex.Message);
        }

        return BadRequest(DataTableResponseMessageModel<ChillpayOperationLogViewEntity[]>.Failed("System Error"));
    }

    [HttpPost("add")]
    public async Task<IActionResult> Add(AddChillpayOperationLogModel model)
    {
        _logger.LogInformation(AppLogEvents.Details, "[Add]Model: {JsonModel}", JsonSerializeObject(model, false));

        try
        {
            var id = await chillpayOperationLogService.AddAsync(model, CancellationToken);
            var response = DataTableResponseMessageModel<long>.Success(id);
            response.TotalRecord = 1;
            response.FilteredRecord = 1;
            return Ok(response);
        }
        catch (InvalidParameterException ex)
        {
            _logger.LogError(AppLogEvents.Error, "[Add]{Error}", ex.Message);
            return BadRequest(DataTableResponseMessageModel<long>.Failed(ex.Message));
        }
        catch (Exception ex)
        {
            _logger.LogError(AppLogEvents.Error, ex, ex.Message);
        }

        return BadRequest(DataTableResponseMessageModel<long>.Failed("System Error"));
    }
}
