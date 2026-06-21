using Application.Common.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Infrastructure.Services;

/// <summary>
/// Resolves tenant information from JWT claims.
/// Provides tenant context globally throughout the request.
/// </summary>
public class CurrentTenantService : ICurrentTenantService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentTenantService(
        IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    /// <summary>
    /// Tenant Id extracted from JWT.
    /// Returns Guid.Empty for SuperAdmin/system requests.
    /// </summary>
    public Guid TenantId
    {
        get
        {
            var tenantClaim = _httpContextAccessor
                .HttpContext?
                .User?
                .FindFirst("tenantId")?
                .Value;

            return Guid.TryParse(tenantClaim, out var tenantId)
                ? tenantId
                : Guid.Empty;
        }
    }

    /// <summary>
    /// Optional tenant code.
    /// Usually passed during login.
    /// </summary>
    public string? TenantCode =>
        _httpContextAccessor.HttpContext?
            .Request?
            .Headers["X-Tenant-Code"]
            .FirstOrDefault();

    /// <summary>
    /// Indicates whether tenant information exists.
    /// </summary>
    public bool IsAvailable =>
        TenantId != Guid.Empty;

    /// <summary>
    /// SuperAdmin bypasses tenant filters.
    /// </summary>
    public bool IsSuperAdmin =>
        _httpContextAccessor.HttpContext?
            .User?
            .IsInRole("SuperAdmin") == true;
}