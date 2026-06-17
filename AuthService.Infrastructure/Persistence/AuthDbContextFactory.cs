using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using NexoraEnterprise.AuthService.Infrastructure.Persistence;

namespace AuthService.Infrastructure.Persistence;

    public class AuthDbContextFactory : IDesignTimeDbContextFactory<AuthDbContext2>
    {
        public AuthDbContext2 CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<AuthDbContext2>();

            optionsBuilder.UseSqlServer(
                "Server=.\\SQLEXPRESS;Database=AuthDb;Trusted_Connection=True;TrustServerCertificate=True");

            return new AuthDbContext2(optionsBuilder.Options);
        }
    }
