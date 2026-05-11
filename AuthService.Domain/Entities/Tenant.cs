
using SharedKernel;
using System.ComponentModel.DataAnnotations;

namespace AuthService.Domain.Entities;
public class Tenant : BaseEntity
{
    

    [Required]
    public string Name { get; set; } = default!;

    public string Subdomain { get; set; } = default!;

    
}