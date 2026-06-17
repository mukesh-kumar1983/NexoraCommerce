using Application.Common.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public class TenantRepository
    : Repository<Tenant>,
      ITenantRepository
{
    public TenantRepository(AuthDbContext context)
        : base(context)
    {
    }

    public async Task<Tenant?> GetBySubdomainAsync(
        string subdomain)
    {
        return await _context.Tenants
            .FirstOrDefaultAsync(x =>
                x.Subdomain == subdomain);
    }

    public async Task<bool> ExistsByNameAsync(
        string name)
    {
        return await _context.Tenants
            .AnyAsync(x => x.Name == name);
    }

    public async Task<bool> ExistsBySubdomainAsync(
        string subdomain)
    {
        return await _context.Tenants
            .AnyAsync(x => x.Subdomain == subdomain);
    }
}