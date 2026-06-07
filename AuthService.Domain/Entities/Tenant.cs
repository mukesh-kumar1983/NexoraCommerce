using NexoraEnterprise.SharedKernel.Common.Models;
using System.ComponentModel.DataAnnotations;

namespace NexoraEnterprise.AuthService.Domain.Entities;
public class Tenant : BaseEntity
{
    

    [Required]
    public string Name { get; set; } = default!;

    public string Subdomain { get; set; } = default!;

    
}