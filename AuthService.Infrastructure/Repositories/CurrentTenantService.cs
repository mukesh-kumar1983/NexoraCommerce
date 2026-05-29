using AuthService.Application.Common.Interfaces;
using Microsoft.AspNetCore.Http;

public class CurrentTenantService : ICurrentTenantService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentTenantService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public Guid TenantId
    {
        get
        {
            var claim = _httpContextAccessor.HttpContext?
                .User.FindFirst("tenant_id")?.Value;

            return Guid.TryParse(claim, out var id)
                ? id
                : Guid.Empty;
        }
    }
}