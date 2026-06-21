using Microsoft.AspNetCore.Authorization;

namespace NexoraEnterprise.SharedInfrastructure.Authorization;

/// <summary>
/// Attribute to enforce permission-based access.
/// </summary>
public class RequiresPermissionAttribute : AuthorizeAttribute
{
    public RequiresPermissionAttribute(string permission)
    {
        Policy = permission;
    }
}