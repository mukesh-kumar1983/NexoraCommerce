using Microsoft.AspNetCore.Identity;

namespace Domain.Entities;

/// <summary>
/// Tenant-aware role (supports SaaS isolation)
/// </summary>
public class Role : IdentityRole<Guid>
{
    public Guid TenantId { get; set; }

    public ICollection<UserRole> UserRoles { get; set; }
        = new List<UserRole>();
}