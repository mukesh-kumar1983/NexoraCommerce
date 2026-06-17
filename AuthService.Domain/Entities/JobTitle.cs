using Domain.Common;
using NexoraEnterprise.SharedKernel.Common.Models;

namespace NexoraEnterprise.AuthService.Domain.Entities
{
    public class JobTitle : BaseEntity, ITenantEntity
    {
        public string Title { get; set; }

        public Guid TenantId { get; set; }
    }
}
