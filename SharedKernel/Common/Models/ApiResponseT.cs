public class ApiResponse<T> : ApiResponse
{
    public T? Data { get; set; }

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

    public new static ApiResponse<T> FailureResponse(
        string errorCode,
        IEnumerable<string> errors,
        string message = "Request failed")
    {
        return new ApiResponse<T>
        {
            Success = false,
            ErrorCode = errorCode,
            Message = message,
            Errors = errors
        };
    }
}