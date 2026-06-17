using Domain.Common;
using NexoraEnterprise.SharedKernel.Common.Models;


    public class JobTitle : BaseEntity, ITenantEntity
    {
        public string Title { get; set; }

        public Guid TenantId { get; set; }
    }

