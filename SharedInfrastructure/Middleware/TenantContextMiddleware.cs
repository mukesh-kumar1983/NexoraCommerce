using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace NexoraEnterprise.SharedInfrastructure.Middleware;

/// <summary>
/// Resolves tenant once per request and stores it in HttpContext items.
/// This removes need for manual tenant passing.
/// </summary>
public class TenantContextMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<TenantContextMiddleware> _logger;

    private const string TenantHeader = "X-Tenant-Id";

    public TenantContextMiddleware(
        RequestDelegate next,
        ILogger<TenantContextMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        var tenantId = context.User.FindFirst("tenantId")?.Value
                       ?? context.Request.Headers[TenantHeader].FirstOrDefault();

        if (!string.IsNullOrEmpty(tenantId))
        {
            context.Items["TenantId"] = tenantId;

            _logger.LogDebug("Tenant resolved: {TenantId}", tenantId);
        }

        await _next(context);
    }
}