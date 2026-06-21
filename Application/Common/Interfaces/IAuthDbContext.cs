using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Application.Common.Interfaces;

/// <summary>
/// Abstraction for Auth database (Clean Architecture)
/// </summary>
public interface IAuthDbContext
{
    // SaaS
    DbSet<Tenant> Tenants { get; }
    DbSet<Module> Modules { get; }
    DbSet<TenantModule> TenantModules { get; }

    // Identity
    DbSet<AppUser> Users { get; }
    DbSet<Role> Roles { get; }
    DbSet<UserRole> UserRoles { get; }

    // HR
    DbSet<UserProfile> UserProfiles { get; }
    DbSet<Department> Departments { get; }
    DbSet<JobTitle> JobTitles { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}