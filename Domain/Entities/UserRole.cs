using Domain.Common;
using Microsoft.AspNetCore.Identity;

namespace Domain.Entities;

/// <summary>
/// Many-to-many mapping between Users and Roles (tenant aware)
/// </summary>
public class UserRole : IdentityUserRole<Guid>, ITenantEntity
{
    public Guid TenantId { get; set; }

    public AppUser User { get; set; } = null!;

    public Role Role { get; set; } = null!;
}