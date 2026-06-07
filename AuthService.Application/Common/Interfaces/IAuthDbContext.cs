using Microsoft.EntityFrameworkCore;
using NexoraEnterprise.AuthService.Domain;
using NexoraEnterprise.AuthService.Domain.Entities;

namespace NexoraEnterprise.AuthService.Application.Common.Interfaces;

public interface IAuthDbContext
{
    DbSet<AppUser> Users { get; }
    DbSet<UserRole> UserRole { get; }

    DbSet<Tenant> Tenant { get; }

    DbSet<Role> Role { get; }

    DbSet<UserProfile> UserProfile { get; }

    DbSet<JobTitle> JobTitle { get; }

    DbSet<Department> Department { get; }


    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}