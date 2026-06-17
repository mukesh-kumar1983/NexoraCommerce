using AuthService.Application.Common.Interfaces;
using AuthService.Infrastructure.Repositories;
using Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NexoraEnterprise.AuthService.Application.Common.Interfaces;
using NexoraEnterprise.AuthService.Infrastruc.Features.Repositories;
using NexoraEnterprise.AuthService.Infrastructure.Messaging;
using NexoraEnterprise.AuthService.Infrastructure.Persistence;

namespace AuthService.API.Extensions;

public static class InfrastructureServiceExtensions
{
    public static IServiceCollection AddInfrastructureServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<AuthDbContext2>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString("DefaultConnection")));

        services.AddScoped<IAuthDbContext>(sp =>
            sp.GetRequiredService<AuthDbContext2>());

        // Repositories
        services.AddScoped<IPasswordHasher, PasswordHasher>();
        services.AddScoped<IJwtTokenService, JwtTokenService>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddSingleton<IMessagePublisher, RabbitMqPublisher>();
        services.AddScoped<IExportService, ExportService>();

        return services;
    }
}