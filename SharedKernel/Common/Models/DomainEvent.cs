using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NexoraEnterprise.SharedKernel.Common.Models
{
    public abstract class DomainEvent
    {
        public DateTime OccurredOn { get; protected set; } = DateTime.UtcNow;
    }
}
