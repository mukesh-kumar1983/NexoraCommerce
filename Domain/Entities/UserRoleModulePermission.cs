using Domain.Common;
using NexoraEnterprise.SharedKernel.Common.Models;

namespace Domain.Entities;

public class UserRoleModulePermission : BaseEntity, ITenantEntity
{
    public Guid TenantId { get; set; }

    public Guid RoleId { get; set; }

    public Guid ModuleId { get; set; }

    public bool CanRead { get; set; } = true;
    public bool CanWrite { get; set; } = false;
    public bool CanDelete { get; set; } = false;
}