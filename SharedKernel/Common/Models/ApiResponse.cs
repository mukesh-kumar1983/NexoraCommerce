namespace NexoraEnterprise.SharedKernel.Common.Models;

public class ApiResponse<T>
{
    public bool Success { get; set; }

    public string Message { get; set; } = string.Empty;

    public T? Data { get; set; }

    public IEnumerable<string>? Errors { get; set; }

    #region Success

    public static ApiResponse<T> SuccessResponse(
        T data,
        string message = "Request successful")
    {
        return new ApiResponse<T>
        {
            Success = true,
            Message = message,
            Data = data
        };
    }

    #endregion

    #region Failure

    public static ApiResponse<T> FailureResponse(
        IEnumerable<string> errors,
        string message = "Request failed")
    {
        return new ApiResponse<T>
        {
            Success = false,
            Message = message,
            Errors = errors
        };
    }

    #endregion
}