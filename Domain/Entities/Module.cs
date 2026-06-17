using NexoraEnterprise.SharedKernel.Common.Models;

namespace Domain.Entities
{
    public class Module : BaseEntity
    {
        public string Name { get; set; } = default!;
        public string Code { get; set; } = default!;
    }
}
