using Domain.Common;
using Microsoft.AspNetCore.Identity;
using NexoraEnterprise.AuthService.Domain.Entities;
using NexoraEnterprise.SharedKernel.Common.Models;

namespace NexoraEnterprise.AuthService.Domain;

public class AppUser : IdentityUser<Guid>, ITenantEntity
{
    public Guid TenantId { get; set; }

    public bool IsLocked { get; set; } = false;

    public DateTime? LastLoginAt { get; set; }

    // Refresh token (MVP level)
    public string? RefreshToken { get; set; }
    public DateTime? RefreshTokenExpiryTime { get; set; }

    // HR profile
    public UserProfile? UserProfile { get; set; }

    public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
}