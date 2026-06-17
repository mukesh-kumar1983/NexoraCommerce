using Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services;

public class ModuleAccessService : IModuleAccessService
{
    private readonly IAuthDbContext _dbContext;
    private readonly ICurrentTenantService _currentTenant;

    public ModuleAccessService(
        IAuthDbContext dbContext,
        ICurrentTenantService currentTenant)
    {
        _dbContext = dbContext;
        _currentTenant = currentTenant;
    }

    // ----------------------------------------------------
    // CHECK IF MODULE IS ENABLED FOR CURRENT TENANT
    // ----------------------------------------------------
    public async Task<bool> IsEnabledAsync(string moduleCode)
    {
        if (_currentTenant.TenantId == null)
            return false;

        var tenantId = _currentTenant.TenantId.Value;

        return await _dbContext.TenantModules
            .AnyAsync(tm =>
                tm.TenantId == tenantId &&
                tm.Module.Code == moduleCode &&
                tm.IsEnabled);
    }

    // ----------------------------------------------------
    // THROW IF MODULE IS NOT ENABLED
    // ----------------------------------------------------
    public async Task EnsureEnabledAsync(string moduleCode)
    {
        var isEnabled = await IsEnabledAsync(moduleCode);

        if (!isEnabled)
        {
            throw new UnauthorizedAccessException(
                $"Module '{moduleCode}' is not enabled for this tenant.");
        }
    }
}