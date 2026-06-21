using Application.Common.Interfaces;
using Domain.Common;
using Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NexoraEnterprise.SharedKernel.Common.Models;

namespace Infrastructure.Persistence;

public class AuthDbContext
    : IdentityDbContext<AppUser, Role, Guid>,
      IAuthDbContext
{
    private readonly ICurrentTenantService _currentTenant;

    public AuthDbContext(
        DbContextOptions<AuthDbContext> options,
        ICurrentTenantService currentTenant)
        : base(options)
    {
        _currentTenant = currentTenant;
    }

    // ===================== SAAS TABLES =====================

    public DbSet<Tenant> Tenants => Set<Tenant>();
    public DbSet<Module> Modules => Set<Module>();
    public DbSet<TenantModule> TenantModules => Set<TenantModule>();

    // Identity + HR tables
    public DbSet<AppUser> Users => Set<AppUser>();
    public DbSet<Role> Roles => Set<Role>();
    public DbSet<UserProfile> UserProfiles => Set<UserProfile>();
    public DbSet<UserRole> UserRoles => Set<UserRole>();

    public DbSet<Department> Departments => Set<Department>();
    public DbSet<JobTitle> JobTitles => Set<JobTitle>();

    // ===================== MODEL CONFIGURATION =====================

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // ---------------- Tenant ----------------
        builder.Entity<Tenant>(entity =>
        {
            entity.HasKey(x => x.Id);

            entity.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(200);

            entity.Property(x => x.Subdomain)
                .HasMaxLength(100);

            entity.HasIndex(x => x.Subdomain)
                .IsUnique();
        });

        builder.Entity<Department>(entity =>
        {
            entity.HasKey(x => x.Id);

            entity.Property(x => x.Title)
                .IsRequired()
                .HasMaxLength(100);

            

            entity.HasIndex(x => new { x.TenantId, x.Title })
                .IsUnique();
        });

        builder.Entity<JobTitle>(entity =>
        {
            entity.HasKey(x => x.Id);

            entity.Property(x => x.Title)
                .IsRequired()
                .HasMaxLength(100);

            entity.HasIndex(x => new { x.TenantId, x.Title })
                .IsUnique();
        });

        // ---------------- Module ----------------
        builder.Entity<Module>(entity =>
        {
            entity.HasKey(x => x.Id);

            entity.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(x => x.Code)
                .IsRequired()
                .HasMaxLength(50);

            entity.HasIndex(x => x.Code)
                .IsUnique();
        });

        // ---------------- TenantModule ----------------
        builder.Entity<TenantModule>(entity =>
        {
            entity.HasKey(x => x.Id);

            entity.HasIndex(x => new { x.TenantId, x.ModuleId })
                .IsUnique();

            entity.HasOne<Tenant>()
                .WithMany()
                .HasForeignKey(x => x.TenantId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne<Module>()
                .WithMany()
                .HasForeignKey(x => x.ModuleId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // ---------------- AppUser ----------------
        builder.Entity<AppUser>(entity =>
        {
            entity.Property(x => x.TenantId)
                .IsRequired();

            entity.HasIndex(x => x.TenantId);
            entity.HasIndex(x => x.Email);
        });

        // ---------------- Role ----------------
        builder.Entity<Role>(entity =>
        {
            entity.HasKey(x => x.Id);

            entity.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(100);

            entity.HasIndex(x => x.Name)
                .IsUnique();

            entity.Property(x => x.TenantId)
                .IsRequired();
        });

        // ---------------- UserProfile (1-1 Identity) ----------------
        builder.Entity<UserProfile>(entity =>
        {
            entity.ToTable("UserProfiles");

            // Primary Key
            entity.HasKey(x => x.Id);

            // 1–1 relationship (Shared Primary Key)
            entity.HasOne(x => x.User)
                .WithOne(x => x.UserProfile)
                .HasForeignKey<UserProfile>(x => x.Id)
                .OnDelete(DeleteBehavior.Cascade);

            // Indexes (performance for SaaS queries)
            entity.HasIndex(x => x.TenantId);
            entity.HasIndex(x => x.DepartmentId);
            entity.HasIndex(x => x.JobTitleId);
                
            // Optional but recommended if multi-tenant is strict
            entity.Property(x => x.TenantId)
                .IsRequired();
        });

        // ---------------- UserRole (RBAC) ----------------
        builder.Entity<UserRole>(entity =>
        {
            entity.HasKey(x => new { x.UserId, x.RoleId });

            entity.HasOne(x => x.User)
                .WithMany(u => u.UserRoles)
                .HasForeignKey(x => x.UserId);

            entity.HasOne(x => x.Role)
                .WithMany(r => r.UserRoles)
                .HasForeignKey(x => x.RoleId);

            entity.Property(x => x.TenantId)
                .IsRequired();

            entity.HasIndex(x => new { x.UserId, x.RoleId, x.TenantId })
                .IsUnique();
        });

        // ===================== GLOBAL FILTERS =====================

        ApplyGlobalFilters(builder);
    }

    // ====================================================
    // GLOBAL FILTER ENGINE
    // ====================================================

    private void ApplyGlobalFilters(ModelBuilder builder)
    {
        foreach (var entityType in builder.Model.GetEntityTypes())
        {
            var clrType = entityType.ClrType;

            // Soft delete
            if (typeof(BaseEntity).IsAssignableFrom(clrType))
            {
                var method = typeof(AuthDbContext)
                    .GetMethod(nameof(SetSoftDeleteFilter),
                        System.Reflection.BindingFlags.NonPublic |
                        System.Reflection.BindingFlags.Static)!
                    .MakeGenericMethod(clrType);

                method.Invoke(null, new object[] { builder });
            }

            // Tenant filter
            if (typeof(ITenantEntity).IsAssignableFrom(clrType))
            {
                var method = typeof(AuthDbContext)
                    .GetMethod(nameof(SetTenantFilter),
                        System.Reflection.BindingFlags.NonPublic |
                        System.Reflection.BindingFlags.Instance)!
                    .MakeGenericMethod(clrType);

                method.Invoke(this, new object[] { builder });
            }
        }
    }

    // ---------------- SOFT DELETE ----------------
    private static void SetSoftDeleteFilter<TEntity>(ModelBuilder builder)
        where TEntity : BaseEntity
    {
        builder.Entity<TEntity>()
            .HasQueryFilter(x => !x.IsDeleted);
    }

    // ---------------- TENANT FILTER ----------------
    private void SetTenantFilter<TEntity>(ModelBuilder builder)
        where TEntity : class, ITenantEntity
    {
        builder.Entity<TEntity>()
            .HasQueryFilter(x =>
                _currentTenant.IsSuperAdmin ||
                x.TenantId == _currentTenant.TenantId);
    }

    // ===================== SAVE CHANGES =====================

    public override async Task<int> SaveChangesAsync(
        CancellationToken cancellationToken = default)
    {
        foreach (var entry in ChangeTracker.Entries())
        {
            // Audit + Soft Delete
            if (entry.Entity is BaseEntity baseEntity)
            {
                if (entry.State == EntityState.Added)
                    baseEntity.CreatedAt = DateTime.UtcNow;

                if (entry.State == EntityState.Modified)
                    baseEntity.ModifiedAt = DateTime.UtcNow;

                if (entry.State == EntityState.Deleted)
                {
                    entry.State = EntityState.Modified;
                    baseEntity.IsDeleted = true;
                    baseEntity.DeletedAt = DateTime.UtcNow;
                }
            }

            // Tenant assignment
            if (entry.Entity is ITenantEntity tenantEntity &&
                entry.State == EntityState.Added)
            {
                // SuperAdmin may create records outside tenant scope
                if (_currentTenant.TenantId == Guid.Empty &&
                    !_currentTenant.IsSuperAdmin)
                {
                    throw new InvalidOperationException(
                        "Tenant is missing for request.");
                }

                tenantEntity.TenantId = _currentTenant.TenantId;
            }
        }

        return await base.SaveChangesAsync(cancellationToken);
    }
}