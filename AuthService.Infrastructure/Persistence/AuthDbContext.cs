using Microsoft.EntityFrameworkCore;
using AuthService.Domain.Entities;
using AuthService.Application.Common.Interfaces;

namespace AuthService.Infrastructure.Persistence;

public class AuthDbContext : DbContext, IAuthDbContext
{
    public AuthDbContext(DbContextOptions<AuthDbContext> options)
        : base(options)
    {
    }

    public DbSet<AppUser> Users { get; set; }
    public DbSet<Role> Role { get; set; }
    public DbSet<Tenant> Tenant { get; set; }

    // ✅ FIX HERE
    public DbSet<UserRole> UserRole { get; set; }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return base.SaveChangesAsync(cancellationToken);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // 🔑 Tenant
        modelBuilder.Entity<Tenant>()
            .HasIndex(t => t.Subdomain)
            .IsUnique();

        // 🔑 User
        modelBuilder.Entity<AppUser>()
            .HasIndex(u => new { u.Email, u.TenantId })
            .IsUnique();

        modelBuilder.Entity<AppUser>()
            .HasOne<Tenant>()
            .WithMany()
            .HasForeignKey(u => u.TenantId)
            .OnDelete(DeleteBehavior.Restrict);



        // 🔑 UserRole (Many-to-Many)
        modelBuilder.Entity<UserRole>()
            .HasKey(ur => new { ur.UserId, ur.RoleId });

        modelBuilder.Entity<UserRole>()
            .HasOne(ur => ur.User)
            .WithMany(u => u.UserRoles)
            .HasForeignKey(ur => ur.UserId);

        modelBuilder.Entity<UserRole>()
            .HasOne(ur => ur.Role)
            .WithMany()
            .HasForeignKey(ur => ur.RoleId);
    }
}
    
