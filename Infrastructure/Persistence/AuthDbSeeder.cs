using Domain.Entities;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence;

public static class AuthDbSeeder
{
    public static async Task SeedAsync(
        AuthDbContext context,
        UserManager<AppUser> userManager,
        RoleManager<IdentityRole<Guid>> roleManager)
    {
        await context.Database.MigrateAsync();

        // ---------------- MODULE SEED ----------------
        if (!await context.Modules.AnyAsync())
        {
            var modules = new List<Module>
            {
                new() { Id = Guid.NewGuid(), Name = "Employee", Code = "EMPLOYEE" },
                new() { Id = Guid.NewGuid(), Name = "HR", Code = "HR" },
                new() { Id = Guid.NewGuid(), Name = "Payroll", Code = "PAYROLL" }
            };

            await context.Modules.AddRangeAsync(modules);
            await context.SaveChangesAsync();
        }

        // ---------------- TENANT SEED ----------------
        if (!await context.Tenants.AnyAsync())
        {
            var tenant = new Tenant
            {
                Id = Guid.NewGuid(),
                Name = "Default Tenant",
                Subdomain = "default"
            };

            await context.Tenants.AddAsync(tenant);
            await context.SaveChangesAsync();

            // ---------------- ASSIGN MODULES ----------------
            var modules = await context.Modules.ToListAsync();

            foreach (var module in modules)
            {
                context.TenantModules.Add(new TenantModule
                {
                    Id = Guid.NewGuid(),
                    TenantId = tenant.Id,
                    ModuleId = module.Id,
                    IsEnabled = true
                });
            }

            await context.SaveChangesAsync();
        }

        // ---------------- ROLE SEED ----------------
        string[] roles = { "SuperAdmin", "Admin", "HR", "User" };

        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new IdentityRole<Guid>
                {
                    Name = role,
                    NormalizedName = role.ToUpper()
                });
            }
        }

        // ---------------- SUPER ADMIN USER ----------------
        var adminEmail = "admin@nexora.com";

        var adminUser = await userManager.FindByEmailAsync(adminEmail);

        if (adminUser == null)
        {
            var user = new AppUser
            {
                Id = Guid.NewGuid(),
                UserName = adminEmail,
                Email = adminEmail,
                EmailConfirmed = true
            };

            var result = await userManager.CreateAsync(user, "Admin@123");

            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(user, "SuperAdmin");
            }
        }
    }
}