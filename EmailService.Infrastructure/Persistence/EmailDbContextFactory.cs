using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace EmailService.Infrastructure.Persistence;

public class EmailDbContextFactory : IDesignTimeDbContextFactory<EmailDbContext>
{
    public EmailDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<EmailDbContext>();

        optionsBuilder.UseSqlServer(
            "Server=.\\SQLEXPRESS;Database=EmailDb;Trusted_Connection=True;TrustServerCertificate=True");

        return new EmailDbContext(optionsBuilder.Options);
    }
}