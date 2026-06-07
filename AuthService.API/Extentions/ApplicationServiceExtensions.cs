using FluentValidation;
using System.Reflection;

namespace NexoraEnterprise.AuthService.API.Extensions;

public static class ApplicationServiceExtensions
{
    public static IServiceCollection AddApplicationServices(
        this IServiceCollection services)
    {
        //var assembly = Assembly.Load("NexoraEnterprise.AuthService.Application");

        var assembly = Assembly.Load("NexoraEnterprise.EmployeeService.Application");

        services.AddMediatR(cfg =>
            cfg.RegisterServicesFromAssembly(assembly));

        services.AddValidatorsFromAssembly(assembly);

        return services;
    }
}