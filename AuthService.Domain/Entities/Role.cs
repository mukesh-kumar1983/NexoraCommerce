using Domain.Common;
using NexoraEnterprise.SharedKernel.Common.Models;
using System.ComponentModel.DataAnnotations;

namespace NexoraEnterprise.AuthService.Domain.Entities;
public class Role : BaseEntity, ITenantEntity
{
   

    [Required]
    public string Name { get; set; } = default!;

    [Required]
    public Guid TenantId { get; set; }
}
