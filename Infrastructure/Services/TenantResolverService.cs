using Application.Common.Interfaces;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services;

/// <summary>
/// Resolves tenant from database using tenantId or tenantCode.
/// </summary>
public class TenantResolverService : ITenantResolverService
{
    private readonly AuthDbContext _context;

    public TenantResolverService(AuthDbContext context)
    {
        _context = context;
    }

    public async Task<Tenant?> ResolveAsync(string? tenantIdentifier)
    {
        if (string.IsNullOrWhiteSpace(tenantIdentifier))
            return null;

        return await _context.Tenants
            .FirstOrDefaultAsync(t =>
                t.Id.ToString() == tenantIdentifier ||
                t.TenantCode == tenantIdentifier);
    }
}