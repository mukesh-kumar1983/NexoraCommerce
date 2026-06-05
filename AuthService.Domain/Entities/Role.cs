using SharedKernel.Common.Models;
using System.ComponentModel.DataAnnotations;

namespace AuthService.Domain.Entities;
public class Role : BaseEntity
{
   

    [Required]
    public string Name { get; set; } = default!;

    [Required]
    public Guid TenantId { get; set; }
}
