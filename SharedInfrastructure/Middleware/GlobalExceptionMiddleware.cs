using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using NexoraEnterprise.SharedKernel.Common.Errors;
using NexoraEnterprise.SharedKernel.Common.Models;
using System.Text.Json;

namespace NexoraEnterprise.SharedInfrastructure.Middleware;

/// <summary>
/// Centralized exception handler for entire API pipeline.
/// Converts all exceptions into standardized ApiResponse format.
/// </summary>
public class GlobalExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionMiddleware> _logger;

    public GlobalExceptionMiddleware(
        RequestDelegate next,
        ILogger<GlobalExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (ValidationException ex)
        {
            await HandleValidationException(context, ex);
        }
        catch (UnauthorizedAccessException ex)
        {
            await HandleException(context, ex, StatusCodes.Status401Unauthorized);
        }
        catch (KeyNotFoundException ex)
        {
            await HandleException(context, ex, StatusCodes.Status404NotFound);
        }
        catch (Exception ex)
        {
            await HandleException(context, ex, StatusCodes.Status500InternalServerError);
        }
    }

    #region Validation Handler

    private async Task HandleValidationException(
        HttpContext context,
        ValidationException ex)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = StatusCodes.Status400BadRequest;

        var errors = ex.Errors?
            .GroupBy(x => x.PropertyName)
            .ToDictionary(
                g => g.Key,
                g => g.Select(x => x.ErrorMessage).ToArray()
            )
            ?? new Dictionary<string, string[]>();

        var response = new ApiResponse<Dictionary<string, string[]>>
        {
            Success = false,
            ErrorCode = ErrorCodes.Validation_Failed,
            Message = "Validation failed",
            Errors = errors.Values.SelectMany(x => x),
            Data = errors
        };

        await WriteResponseAsync(context, response);
    }

    #endregion

    #region General Exception Handler

    private async Task HandleException(
        HttpContext context,
        Exception ex,
        int statusCode)
    {
        _logger.LogError(
            ex,
            "Unhandled exception. TraceId: {TraceId}",
            context.TraceIdentifier);

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = statusCode;

        var (errorCode, message) = statusCode switch
        {
            StatusCodes.Status401Unauthorized => (ErrorCodes.Auth_Unauthorized, "Unauthorized access"),
            StatusCodes.Status404NotFound => (ErrorCodes.General_NotFound, "Resource not found"),
            _ => (ErrorCodes.General_ServerError, "An unexpected error occurred")
        };

        var response = new ApiResponse<object>
        {
            Success = false,
            ErrorCode = errorCode,
            Message = message,
            Errors = new[] { ex.Message },
            Data = new
            {
                traceId = context.TraceIdentifier
            }
        };

        await WriteResponseAsync(context, response);
    }

    #endregion

    #region Response Writer

    private static async Task WriteResponseAsync<T>(
        HttpContext context,
        ApiResponse<T> response)
    {
        var options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = false
        };

        var json = JsonSerializer.Serialize(response, options);

        await context.Response.WriteAsync(json);
    }

    #endregion
}