using Domain.Entities;

namespace Application.Common.Interfaces;

public interface ITenantRepository : IRepository<Tenant>
{
    Task<Tenant?> GetBySubdomainAsync(string subdomain);

    Task<bool> ExistsByNameAsync(string name);

    Task<bool> ExistsBySubdomainAsync(string subdomain);
}