using AuthService.Infrastructure.Persistence;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using NexoraEnterprise.AuthService.Infrastructure.Persistence;
using NexoraEnterprise.SharedInfrastructure.Middleware;
using Serilog;

namespace NexoraEnterprise.AuthService.API.Extensions;

public static class ApplicationBuilderExtensions
{
    public static void UseApplicationPipeline(
        this WebApplication app,
        IWebHostEnvironment env)
    {
        // Exception handling FIRST
        app.UseMiddleware<GlobalExceptionMiddleware>();

        // Serilog request logging
        app.UseSerilogRequestLogging(options =>
        {
            options.MessageTemplate =
                "HTTP {RequestMethod} {RequestPath} responded {StatusCode} in {Elapsed:0.0000}ms";

            options.EnrichDiagnosticContext = (diag, http) =>
            {
                diag.Set("TraceId", http.TraceIdentifier);
                diag.Set("ClientIP", http.Connection.RemoteIpAddress);
                diag.Set("UserAgent", http.Request.Headers["User-Agent"]);
            };
        });

        if (env.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();
    }

    public static async Task MigrateAndSeedDatabaseAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();

        var context = scope.ServiceProvider.GetRequiredService<AuthDbContext2>();

        await context.Database.MigrateAsync();

        //await AuthDbSeeder.SeedAsync(context);
    }
}