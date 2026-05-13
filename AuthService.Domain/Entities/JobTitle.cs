using SharedKernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthService.Domain.Entities
{
    public class JobTitle : BaseEntity
    {
        public string Title { get; set; }

        public Guid TenantId { get; set; }
    }
}
