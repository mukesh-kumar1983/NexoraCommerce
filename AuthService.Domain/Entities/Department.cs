using NexoraEnterprise.SharedKernel.Common.Models;

namespace NexoraEnterprise.AuthService.Domain.Entities
{
    public class Department : BaseEntity
    {
        public string Title { get; set; }

        public Guid TenantId { get; set; }
    }
}
