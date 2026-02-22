namespace HCAS.WasmApp.Models;

public class Result<T>
{
    public bool IsSuccess { get; set; }
    public bool IsError => !IsSuccess;
    public bool IsValidationError() => Type == EnumRespType.ValidationError;
    public bool IsSystemError() => Type == EnumRespType.SystemError;
    public bool IsNotFound() => Type == EnumRespType.NotFound;

    public EnumRespType Type { get; set; }
    public T? Data { get; set; }
    public string Message { get; set; } = string.Empty;

    public static Result<T> Success(T data, string message = "Success") => new() { IsSuccess = true, Type = EnumRespType.Success, Data = data, Message = message };
    public static Result<T> DeleteSuccess(string message = "Deleting Successful.") => new() { IsSuccess = true, Type = EnumRespType.Success, Message = message };
    public static Result<T> ValidationError(string message, T? data = default) => new() { IsSuccess = false, Type = EnumRespType.ValidationError, Data = data, Message = message };
    public static Result<T> SystemError(string message, T? data = default) => new() { IsSuccess = false, Type = EnumRespType.SystemError, Data = data, Message = message };
    public static Result<T> NotFound(string message, T? data = default) => new() { IsSuccess = false, Type = EnumRespType.NotFound, Data = data, Message = message };
}

public enum EnumRespType
{
    None,
    Success,
    ValidationError,
    SystemError,
    NotFound
}

public class PagedResult<T>
{
    public IEnumerable<T> Items { get; set; } = new List<T>();
    public int TotalCount { get; set; }
}
