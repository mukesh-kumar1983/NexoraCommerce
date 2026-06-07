using NexoraEnterprise.AuthService.Domain.Entities;
using NexoraEnterprise.SharedKernel.Common.Models;

namespace NexoraEnterprise.AuthService.Domain;
public class AppUser : BaseEntity
{
    public string Email { get; set; } = default!;
    public string PasswordHash { get; set; } = default!;
    public bool IsLocked { get; set; } = false;

    public string? RefreshToken { get; set; }
    public DateTime? RefreshTokenExpiryTime { get; set; }

    public Guid TenantId { get; set; }

    public UserProfile? UserProfile { get; set; }

    public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
}