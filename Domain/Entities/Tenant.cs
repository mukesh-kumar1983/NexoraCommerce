using NexoraEnterprise.SharedKernel.Common.Models;

namespace Domain.Entities
{
    public class Tenant : BaseEntity
    {
        public string Name { get; set; } = default!;
        public string Subdomain { get; set; } = default!;

        public string TenantCode { get; set; } = default!; // 👈 ADD THIS
    }
}
