using Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace NexoraEnterprise.AuthService.Domain.Entities;

public class Role : IdentityRole<Guid>
{
    public Guid TenantId { get; set; }

    public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
}