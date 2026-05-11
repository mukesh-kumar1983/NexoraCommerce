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
            #region Tenant Seed

            // =========================
            // 1. Tenant
            // =========================
            var tenant = await context.Tenant
                .FirstOrDefaultAsync(t => t.Subdomain == "default");

            if (tenant == null)
            {
                tenant = new Tenant
                {
                    Id = Guid.NewGuid(),
                    Name = "Default Tenant",
                    Subdomain = "default"
                };

                context.Tenant.Add(tenant);
            }

            #endregion

            #region Roles Seed

            // =========================
            // 2. Roles
            // =========================
            var adminRole = await context.Role
                .FirstOrDefaultAsync(r => r.Name == "Admin");

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

            var userRole = await context.Role
                .FirstOrDefaultAsync(r => r.Name == "User");

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

            #endregion

            #region Admin User Seed

            // =========================
            // 3. Admin User
            // =========================
            var adminUser = await context.Users
                .FirstOrDefaultAsync(u => u.Email == "mk_soni@hotmail.com");

            if (adminUser == null)
            {
                adminUser = new AppUser
                {
                    Id = Guid.NewGuid(),
                    Email = "mk_soni@hotmail.com",
                    FirstName = "System",
                    LastName = "Admin",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin"),
                    TenantId = tenant.Id,
                    CreatedBy = "System",
                    ModifiedBy = "System"
                };

                context.Users.Add(adminUser);
            }

            #endregion

            #region User Role Mapping Seed

            // =========================
            // 4. User Role Mapping
            // =========================
            var alreadyAssigned = await context.UserRole
                .AnyAsync(ur =>
                    ur.UserId == adminUser.Id &&
                    ur.RoleId == adminRole.Id);

            if (!alreadyAssigned)
            {
                context.UserRole.Add(new UserRole
                {
                    UserId = adminUser.Id,
                    RoleId = adminRole.Id,
                    TenantId = tenant.Id
                });
            }

            #endregion

            // =========================
            // Save All Changes
            // =========================
            await context.SaveChangesAsync();

            // =========================
            // Commit Transaction
            // =========================
            await transaction.CommitAsync();
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }
}