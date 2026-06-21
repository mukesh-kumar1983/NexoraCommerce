using Microsoft.AspNetCore.Authorization;

namespace NexoraEnterprise.SharedInfrastructure.Authorization;

/// <summary>
/// Centralized policy registration helper.
/// </summary>
public static class PolicyProvider
{
    public static void AddPermissionPolicies(AuthorizationOptions options)
    {
        options.AddPolicy("Employee.Read", policy =>
            policy.RequireClaim("permission", "employee.read"));

        options.AddPolicy("Employee.Create", policy =>
            policy.RequireClaim("permission", "employee.create"));

        options.AddPolicy("Department.Read", policy =>
            policy.RequireClaim("permission", "department.read"));
    }
}