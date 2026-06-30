using ChillPay.Core.Models.Enums;
using System.Text.Json.Serialization;

namespace ChillPay.Core.Models;

public class ApiResponseModel : ApiResponseModel<string>
{
    public ApiResponseModel(ChillPayResponseCode status, string message)
    {
        Status = status;
        Message = message;
    }

    public static new ApiResponseModel Success(string data = "OK", string message = nameof(ChillPayResponseCode.Success))
    {
        return new ApiResponseModel(ChillPayResponseCode.Success, message)
        {
            TotalRecord = 1,
            Data = data,
        };
    }

    public static new ApiResponseModel Failed(string message = "Invalid Parameter")
    {
        return new ApiResponseModel(ChillPayResponseCode.InvalidParameter, message) { Data = null };
    }

    public static ApiResponseModel Error(string message = "System Error")
    {
        return new ApiResponseModel(ChillPayResponseCode.SystemError, message) { Data = null };
    }
}

public class ApiResponseModel<T> where T : class
{
    public ApiResponseModel() { }

    public ApiResponseModel(ChillPayResponseCode status, string message)
    {
        Status = status;
        Message = message;
    }

    public ApiResponseModel(T content, ChillPayResponseCode status, string message)
    {
        Status = status;
        Message = message;
        Data = content;
        TotalRecord = 1;
    }

    public static ApiResponseModel<T> Success(T content, string message = nameof(ChillPayResponseCode.Success))
    {
        return new ApiResponseModel<T>(content, ChillPayResponseCode.Success, message);
    }

    public static ApiResponseModel<T> Success(T content, long totalRecord)
    {
        return new ApiResponseModel<T>(content, ChillPayResponseCode.Success, nameof(ChillPayResponseCode.Success))
        {
            TotalRecord = totalRecord,
        };
    }

    public static ApiResponseModel<T> Failed(string message = "Invalid Parameter")
    {
        return new ApiResponseModel<T>(ChillPayResponseCode.InvalidParameter, message);
    }

    [JsonPropertyName("status")]
    public ChillPayResponseCode Status { get; set; }

    [JsonPropertyName("message")]
    public string Message { get; set; }

    [JsonPropertyName("totalRecord")]
    public long TotalRecord { get; set; }

    [JsonPropertyName("data")]
    public T Data { get; set; }

    public bool IsSucceeded() => Status == ChillPayResponseCode.Success;
}
