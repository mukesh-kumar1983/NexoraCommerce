public class ApiResponse
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;

    /// <summary>
    /// Machine-readable error code (Stripe-style).
    /// </summary>
    public string? ErrorCode { get; set; }

    public IEnumerable<string>? Errors { get; set; }

    public static ApiResponse SuccessResponse(string message = "Request successful")
    {
        return new ApiResponse
        {
            Success = true,
            Message = message
        };
    }

    public static ApiResponse FailureResponse(
        string errorCode,
        IEnumerable<string> errors,
        string message = "Request failed")
    {
        return new ApiResponse
        {
            Success = false,
            ErrorCode = errorCode,
            Message = message,
            Errors = errors
        };
    }
}