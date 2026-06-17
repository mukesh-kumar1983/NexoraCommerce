using Application.Common.Interfaces;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services;

public class ModuleAuthorizationService : IModuleAuthorizationService
{
    private readonly AuthDbContext _context;
    private readonly ICurrentTenantService _currentTenant;

    public ModuleAuthorizationService(
        AuthDbContext context,
        ICurrentTenantService currentTenant)
    {
        _context = context;
        _currentTenant = currentTenant;
    }

    public bool HasModuleAccess(string moduleCode)
    {
        if (_currentTenant.IsSuperAdmin)
            return true;

        return _context.TenantModules
            .Include(x => x.Module)
            .Any(x =>
                x.TenantId == _currentTenant.TenantId &&
                x.Module.Code == moduleCode &&
                x.IsEnabled);
    }

    public bool CanAccess(string moduleCode, string permission)
    {
        // For now simplified
        // Later we extend with RolePermission table

        return HasModuleAccess(moduleCode);
    }
}