using Domain.Common;
using NexoraEnterprise.SharedKernel.Common.Models;
using System.ComponentModel.DataAnnotations;

namespace NexoraEnterprise.AuthService.Domain.Entities;
public class Tenant : BaseEntity
{
    

    [Required]
    public string Name { get; set; } = default!;

    public string TenantCode { get; set; } = default!;

    
}