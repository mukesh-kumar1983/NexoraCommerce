using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using NexoraEnterprise.AuthService.Domain;
using NexoraEnterprise.AuthService.Domain.Entities;

namespace Application.Common.Interfaces;

public interface IAuthDbContext
{
    DbSet<Tenant> Tenants { get; }
    DbSet<Module> Modules { get; }
    DbSet<TenantModule> TenantModules { get; }

    DbSet<UserProfile> UserProfiles { get; }
    DbSet<AppUser> Users { get; }
    DbSet<UserRole> UserRoles { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}