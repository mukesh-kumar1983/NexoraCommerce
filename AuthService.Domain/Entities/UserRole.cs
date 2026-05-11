using System.ComponentModel.DataAnnotations;

namespace AuthService.Domain.Entities;
public class UserRole
{
    [Key]
    public Guid UserId { get; set; }

    [Required]
    public AppUser User { get; set; } = default!;

    [Key]
    public Guid RoleId { get; set; }

    [Required]
    public Role Role { get; set; } = default!;

    [Required]
    public Guid TenantId { get; set; }
}