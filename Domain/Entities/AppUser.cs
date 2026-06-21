using Domain.Common;
using Microsoft.AspNetCore.Identity;

namespace Domain.Entities;

/// <summary>
/// Authentication identity only (NOT business data).
/// Business data lives in UserProfile.
/// </summary>
public class AppUser : IdentityUser<Guid>, ITenantEntity
{

    public Guid TenantId { get; set; }

    public bool IsActive { get; set; } = true;

    public bool IsDeleted { get; set; }

    public DateTime? LastLoginAt { get; set; }

    public string? RefreshToken { get; set; }

    public DateTime? RefreshTokenExpiryTime { get; set; }

    // 1-1 relationship with UserProfile
    public UserProfile? UserProfile { get; set; }

    // Convenience accessor (no DB mapping)
    public string? FullName =>
        UserProfile != null
            ? $"{UserProfile.FirstName} {UserProfile.LastName}"
            : UserName;

    public ICollection<UserRole> UserRoles { get; set; }
        = new List<UserRole>();
}