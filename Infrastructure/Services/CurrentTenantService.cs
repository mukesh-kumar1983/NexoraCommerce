using Application.Common.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Infrastructure.Services;

/// <summary>
/// ------------------------------------------------------------
/// CurrentTenantService (CORE SAAS INFRASTRUCTURE SERVICE)
/// ------------------------------------------------------------
/// 
/// This service is responsible for resolving the "current tenant"
/// for every incoming HTTP request in a multi-tenant SaaS system.
///
/// 🧠 Key Responsibilities:
/// - Identify which tenant is making the request
/// - Provide tenant context across Application layer
/// - Support both JWT-based and middleware-based resolution
/// - Ensure safe fallback when middleware is not used
///
/// ------------------------------------------------------------
/// RESOLUTION PRIORITY ORDER:
/// ------------------------------------------------------------
/// 1. Middleware (SetTenant) → highest priority (runtime override)
/// 2. JWT Claims → fallback identity-based resolution
/// 3. Null → no tenant context (system-level access)
/// ------------------------------------------------------------
/// </summary>
public class CurrentTenantService : ICurrentTenantService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    /// <summary>
    /// Runtime cached tenant values.
    /// These are set by middleware at the start of each request.
    /// </summary>
    private Guid? _tenantId;
    private string? _subdomain;
    private bool _isSuperAdmin;

    public CurrentTenantService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    // ----------------------------------------------------
    // TENANT IDENTIFIER (PRIMARY CONTEXT VALUE)
    // ----------------------------------------------------
    /// <summary>
    /// Gets the current TenantId for the request.
    /// 
    /// Resolution Flow:
    /// 1. If middleware has already set tenant → use it
    /// 2. Otherwise fallback to JWT claim ("tenantId")
    /// 
    /// ⚠️ Important:
    /// TenantId is used for:
    /// - Data filtering (multi-tenancy isolation)
    /// - Authorization decisions
    /// - Auditing
    /// </summary>
    public Guid? TenantId
    {
        get
        {
            // 1. Middleware override (preferred source)
            if (_tenantId.HasValue)
                return _tenantId;

            // 2. Fallback to JWT claim
            var value = _httpContextAccessor.HttpContext?
                .User?.FindFirst("tenantId")?.Value;

            return Guid.TryParse(value, out var id) ? id : null;
        }
    }

    // ----------------------------------------------------
    // TENANT SUBDOMAIN (SAAS ROUTING SUPPORT)
    // ----------------------------------------------------
    /// <summary>
    /// Subdomain is used for SaaS routing:
    /// Example: acme.yourapp.com → "acme"
    /// 
    /// This helps resolve tenant without authentication.
    /// </summary>
    public string? Subdomain =>
        _subdomain ??
        _httpContextAccessor.HttpContext?
            .User?.FindFirst("subdomain")?.Value;

    // ----------------------------------------------------
    // SUPER ADMIN FLAG
    // ----------------------------------------------------
    /// <summary>
    /// Indicates whether the current user is a Super Admin.
    /// 
    /// Super Admin bypasses:
    /// - Tenant filtering
    /// - Module restrictions
    /// - Tenant isolation rules
    /// </summary>
    public bool IsSuperAdmin =>
        _isSuperAdmin ||
        _httpContextAccessor.HttpContext?
            .User?.FindFirst("isSuperAdmin")?.Value == "true";

    // ----------------------------------------------------
    // CALLED BY MIDDLEWARE (REQUEST INITIALIZATION)
    // ----------------------------------------------------
    /// <summary>
    /// This method is called once per HTTP request by TenantMiddleware.
    /// 
    /// It sets the runtime tenant context BEFORE application logic executes.
    /// 
    /// This ensures:
    /// - Consistent tenant resolution
    /// - Avoids repeated JWT parsing
    /// - Allows middleware override (subdomain/header resolution)
    /// </summary>
    public void SetTenant(Guid? tenantId, string? subdomain, bool isSuperAdmin)
    {
        _tenantId = tenantId;
        _subdomain = subdomain;
        _isSuperAdmin = isSuperAdmin;
    }

    // ----------------------------------------------------
    // INTERNAL HELPER: CLAIM READER
    // ----------------------------------------------------
    /// <summary>
    /// Reads a string claim from JWT token safely.
    /// </summary>
    private string? ResolveFromClaims(string key)
    {
        return _httpContextAccessor.HttpContext?
            .User?.FindFirst(key)?.Value;
    }

    // ----------------------------------------------------
    // INTERNAL HELPER: BOOLEAN CLAIM READER
    // ----------------------------------------------------
    /// <summary>
    /// Reads a boolean claim from JWT token safely.
    /// Used for flags like "isSuperAdmin".
    /// </summary>
    private bool ResolveBoolFromClaims(string key)
    {
        return _httpContextAccessor.HttpContext?
            .User?.FindFirst(key)?.Value == "true";
    }
}