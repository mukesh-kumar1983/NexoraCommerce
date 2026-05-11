using Microsoft.EntityFrameworkCore;
using AuthService.Domain.Entities;

namespace AuthService.Application.Common.Interfaces;

public interface IAuthDbContext
{
    DbSet<AppUser> Users { get; }
    DbSet<UserRole> UserRole { get; }

    DbSet<Tenant> Tenant { get; }

    DbSet<Role> Role { get; }


    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}