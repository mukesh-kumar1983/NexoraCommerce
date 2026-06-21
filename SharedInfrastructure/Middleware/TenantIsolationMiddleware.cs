using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using NexoraEnterprise.SharedKernel.Common.Errors;
using NexoraEnterprise.SharedKernel.Common.Models;
using System.Security.Claims;
using System.Text.Json;

namespace NexoraEnterprise.SharedInfrastructure.Middleware;

/// <summary>
/// Enforces SaaS tenant isolation across all requests.
/// Prevents cross-tenant access by validating JWT tenant vs request tenant.
/// </summary>
public class TenantIsolationMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<TenantIsolationMiddleware> _logger;

    private const string TenantHeader = "X-Tenant-Id";

    public TenantIsolationMiddleware(
        RequestDelegate next,
        ILogger<TenantIsolationMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        // Skip for unauthenticated endpoints like login
        if (!context.User.Identity?.IsAuthenticated ?? true)
        {
            await _next(context);
            return;
        }

        var userTenantId = context.User.FindFirst("tenantId")?.Value;
        var requestTenantId = context.Request.Headers[TenantHeader].FirstOrDefault();

        // If missing tenant in request
        if (string.IsNullOrEmpty(requestTenantId))
        {
            await WriteError(context, "TENANT_HEADER_MISSING", "Tenant header is required");
            return;
        }

        // If JWT missing tenant
        if (string.IsNullOrEmpty(userTenantId))
        {
            await WriteError(context, "TENANT_CLAIM_MISSING", "Tenant claim missing in token");
            return;
        }

        // Cross-tenant protection
        if (!string.Equals(userTenantId, requestTenantId, StringComparison.OrdinalIgnoreCase))
        {
            _logger.LogWarning(
                "Cross-tenant access blocked. UserTenant: {UserTenant}, RequestTenant: {RequestTenant}",
                userTenantId,
                requestTenantId);

            await WriteError(context, ErrorCodes.Tenant_Mismatch, "Cross-tenant access denied");
            return;
        }

        await _next(context);
    }

    private async Task WriteError(HttpContext context, string errorCode, string message)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = StatusCodes.Status403Forbidden;

        var response = new ApiResponse<object>
        {
            Success = false,
            ErrorCode = errorCode,
            Message = message,
            Errors = new[] { message },
            Data = new
            {
                traceId = context.TraceIdentifier
            }
        };

        var json = JsonSerializer.Serialize(response, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });

        await context.Response.WriteAsync(json);
    }
}