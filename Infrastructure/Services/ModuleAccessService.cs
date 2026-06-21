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
        // 1. SuperAdmin bypasses everything
        if (_currentTenant.IsSuperAdmin)
            return true;

        // 2. Tenant must exist for normal users
        if (_currentTenant.TenantId == Guid.Empty)
            return false;

        var tenantId = _currentTenant.TenantId;

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