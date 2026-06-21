namespace Application.Common.Interfaces;

/// <summary>
/// Provides current request tenant context globally.
/// </summary>
public interface ICurrentTenantService
{
    Guid TenantId { get; }
    string? TenantCode { get; }
    bool IsAvailable { get; }

    bool IsSuperAdmin { get; }
}