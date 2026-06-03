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

    public DbSet<UserRole> UserRole { get; set; }
    public DbSet<UserProfile> UserProfile { get; set; }

    public DbSet<Department> Department { get; set; }
    public DbSet<JobTitle> JobTitle { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // =========================
        // TENANT CONFIG
        // =========================
        modelBuilder.Entity<Tenant>()
            .HasIndex(t => t.Subdomain)
            .IsUnique();

        // =========================
        // APP USER CONFIG
        // =========================
        modelBuilder.Entity<AppUser>()
            .HasIndex(u => new { u.Email, u.TenantId })
            .IsUnique();

        modelBuilder.Entity<AppUser>()
            .HasOne<Tenant>()
            .WithMany()
            .HasForeignKey(u => u.TenantId)
            .OnDelete(DeleteBehavior.Restrict);

        // =========================
        // USER PROFILE (1:1 FIXED)
        // =========================

        modelBuilder.Entity<UserProfile>()
    .HasKey(p => p.Id);

        modelBuilder.Entity<AppUser>()
    .HasOne(u => u.UserProfile)
    .WithOne(p => p.User)
    .HasForeignKey<UserProfile>(p => p.Id)   // PK = FK
    .OnDelete(DeleteBehavior.Cascade);

        // =========================
        // USER ROLE (MANY TO MANY)
        // =========================

        modelBuilder.Entity<UserRole>()
            .HasKey(ur => new { ur.UserId, ur.RoleId });

        modelBuilder.Entity<UserRole>()
            .HasOne(ur => ur.User)
            .WithMany(u => u.UserRoles)
            .HasForeignKey(ur => ur.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<UserRole>()
            .HasOne(ur => ur.Role)
            .WithMany()
            .HasForeignKey(ur => ur.RoleId)
            .OnDelete(DeleteBehavior.Cascade);
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return base.SaveChangesAsync(cancellationToken);
    }
}