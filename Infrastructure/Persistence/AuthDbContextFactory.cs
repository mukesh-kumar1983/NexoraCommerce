using Application.Common.Interfaces;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Infrastructure.Persistence;

public class AuthDbContextFactory : IDesignTimeDbContextFactory<AuthDbContext>
{
    public AuthDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<AuthDbContext>();

        optionsBuilder.UseSqlServer(
            "Server=.;Database=NexoraAuthDb;Trusted_Connection=True;TrustServerCertificate=True;");

        // Fake tenant service for migrations
        var fakeTenant = new FakeTenantService();

        return new AuthDbContext(optionsBuilder.Options, fakeTenant);
    }
}

// ----------------------------------------------------
// DESIGN TIME FAKE SERVICE
// ----------------------------------------------------
public class FakeTenantService : ICurrentTenantService
{
    public Guid? TenantId => null;
    public string? Subdomain => null;
    public bool IsSuperAdmin => true;

    public void SetTenant(Guid? tenantId, string? subdomain, bool isSuperAdmin)
    {
        // no-op
    }
}