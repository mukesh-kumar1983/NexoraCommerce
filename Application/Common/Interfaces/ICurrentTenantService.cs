namespace Application.Common.Interfaces;

public interface ICurrentTenantService
{
    Guid? TenantId { get; }
    string? Subdomain { get; }
    bool IsSuperAdmin { get; }

    void SetTenant(Guid? tenantId, string? subdomain, bool isSuperAdmin);
}