namespace ChillPay.OperationLogsApi.Models;

public enum ApiResponseStatus
{
    Success = 0,
    Fail = 1,
    Error = 2,
    SystemError = 3,
}

public class DataTableResponseMessageModel<T>
{
    public ApiResponseStatus Status { get; set; }
    public string Message { get; set; }
    public long TotalRecord { get; set; }
    public T Data { get; set; }
    public int Draw { get; set; }
    public int Start { get; set; }
    public int PageSize { get; set; }
    public long FilteredRecord { get; set; }

    public static DataTableResponseMessageModel<T> Success(T data)
    {
        return new DataTableResponseMessageModel<T>
        {
            Status = ApiResponseStatus.Success,
            Message = "Success",
            Data = data,
        };
    }

    public static DataTableResponseMessageModel<T> Failed(string message)
    {
        return new DataTableResponseMessageModel<T>
        {
            Status = ApiResponseStatus.Fail,
            Message = message,
            Data = default,
        };
    }
}
