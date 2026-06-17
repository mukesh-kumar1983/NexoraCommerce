using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using NexoraEnterprise.AuthService.Domain.Entities;

namespace Domain.Entities;

public class UserRole : IdentityUserRole<Guid>
{
    // SaaS isolation
    public Guid TenantId { get; set; }

    // Navigation (optional but useful)
    public AppUser User { get; set; } = null!;
    public Role Role { get; set; } = null!;
}