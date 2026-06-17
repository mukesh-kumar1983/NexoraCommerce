using NexoraEnterprise.SharedKernel.Common.Models;

namespace Domain.Entities
{
    public class TenantModule : BaseEntity
    {
        public Guid TenantId { get; set; }
        public Guid ModuleId { get; set; }

        public bool IsEnabled { get; set; } = true;

        public Module Module { get; set; } = null!;
    }
}
