using Domain.Entities;

namespace Application.Common.Interfaces;

/// <summary>
/// Resolves tenant context for authentication and requests.
/// </summary>
public interface ITenantResolverService
{
    Task<Tenant?> ResolveAsync(string? tenantIdentifier);
}