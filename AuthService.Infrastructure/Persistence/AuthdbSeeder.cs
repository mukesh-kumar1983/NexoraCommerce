using AuthService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace AuthService.Infrastructure.Persistence;

public static class AuthDbSeeder
{
    public static async Task SeedAsync(AuthDbContext context)
    {
        await context.Database.MigrateAsync();

        await using var transaction = await context.Database.BeginTransactionAsync();

        try
        {
            var tenant = await SeedTenant(context);
            var roles = await SeedRoles(context, tenant);
            var adminUser = await SeedAdminUser(context, tenant);
            await SeedUserRole(context, tenant, adminUser, roles.adminRole);

            await SeedDepartments(context, tenant);
            await SeedJobTitles(context, tenant);

            await context.SaveChangesAsync();
            await transaction.CommitAsync();
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    #region Tenant

    private static async Task<Tenant> SeedTenant(AuthDbContext context)
    {
        var tenant = await context.Tenant
            .FirstOrDefaultAsync(x => x.Subdomain == "default");

        if (tenant != null)
            return tenant;

        tenant = new Tenant
        {
            Id = Guid.NewGuid(),
            Name = "Default Tenant",
            Subdomain = "default"
        };

        context.Tenant.Add(tenant);
        return tenant;
    }

    #endregion

    #region Roles

    private static async Task<(Role adminRole, Role userRole)> SeedRoles(AuthDbContext context, Tenant tenant)
    {
        var adminRole = await context.Role.FirstOrDefaultAsync(x => x.Name == "Admin" && x.TenantId == tenant.Id);
        var userRole = await context.Role.FirstOrDefaultAsync(x => x.Name == "User" && x.TenantId == tenant.Id);

        if (adminRole == null)
        {
            adminRole = new Role
            {
                Id = Guid.NewGuid(),
                Name = "Admin",
                TenantId = tenant.Id
            };
            context.Role.Add(adminRole);
        }

        if (userRole == null)
        {
            userRole = new Role
            {
                Id = Guid.NewGuid(),
                Name = "User",
                TenantId = tenant.Id
            };
            context.Role.Add(userRole);
        }

        return (adminRole, userRole);
    }

    #endregion

    #region Admin User

    private static async Task<AppUser> SeedAdminUser(AuthDbContext context, Tenant tenant)
    {
        var adminUser = await context.Users
            .FirstOrDefaultAsync(x => x.Email == "admin@system.com");

        if (adminUser != null)
            return adminUser;

        adminUser = new AppUser
        {
            Id = Guid.NewGuid(),
            Email = "admin@system.com",
            //FirstName = "System",
            //LastName = "Admin",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin@123"),
            TenantId = tenant.Id,
            CreatedBy = "SYSTEM",
            ModifiedBy = "SYSTEM"
        };

        context.Users.Add(adminUser);

        context.UserProfile.Add(new UserProfile
        {
            Id = adminUser.Id,
            FirstName = "SYSTEM",
            LastName = "ADMIN",
            Gender = Gender.Male,
            DepartmentId = null,
            JobTitleId = null,
            TenantId = tenant.Id,

        });

        return adminUser;
    }

    #endregion

    #region UserRole Mapping

    private static async Task SeedUserRole(AuthDbContext context, Tenant tenant, AppUser user, Role adminRole)
    {
        var exists = await context.UserRole
            .AnyAsync(x => x.UserId == user.Id && x.RoleId == adminRole.Id);

        if (!exists)
        {
            context.UserRole.Add(new UserRole
            {
                UserId = user.Id,
                RoleId = adminRole.Id,
                TenantId = tenant.Id
            });
        }
    }

    #endregion

    #region Departments

    private static async Task SeedDepartments(AuthDbContext context, Tenant tenant)
    {
        var exists = await context.Department.AnyAsync(x => x.TenantId == tenant.Id);
        if (exists) return;

        context.Department.AddRange(
            new Department { Id = Guid.NewGuid(), Title = "Development", TenantId = tenant.Id },
            new Department { Id = Guid.NewGuid(), Title = "HR", TenantId = tenant.Id },
            new Department { Id = Guid.NewGuid(), Title = "Finance", TenantId = tenant.Id }
        );
    }

    #endregion

    #region JobTitles

    private static async Task SeedJobTitles(AuthDbContext context, Tenant tenant)
    {
        var exists = await context.JobTitle.AnyAsync(x => x.TenantId == tenant.Id);
        if (exists) return;

        context.JobTitle.AddRange(
            new JobTitle { Id = Guid.NewGuid(), Title = "Developer", TenantId = tenant.Id },
            new JobTitle { Id = Guid.NewGuid(), Title = "Senior Developer", TenantId = tenant.Id },
            new JobTitle { Id = Guid.NewGuid(), Title = "Manager", TenantId = tenant.Id }
        );
    }

    #endregion
}