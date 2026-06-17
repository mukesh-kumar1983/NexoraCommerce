using Domain.Common;
using Microsoft.AspNetCore.Identity;

namespace Domain.Entities;

public class AppUser : IdentityUser<Guid>, ITenantEntity
{
    public Guid TenantId { get; set; }

    public bool IsActive { get; set; } = true;

    public DateTime? LastLoginAt { get; set; }

    public string? RefreshToken { get; set; }
    public DateTime? RefreshTokenExpiryTime { get; set; }

    public UserProfile? UserProfile { get; set; }

    public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
}