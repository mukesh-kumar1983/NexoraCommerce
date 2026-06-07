using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NexoraEnterprise.SharedKernel.Common.Exports
{
    public class ExportRequest
    {
        // Grid state mapping (VERY IMPORTANT)
        public string? Search { get; set; }

        public string? SortField { get; set; }

        public string? SortDir { get; set; }

        // Flexible filters for any module (Employees, Departments, etc.)
        public Dictionary<string, string>? Filters { get; set; }

        // Optional: tenant safety (useful in SaaS)
        public Guid? TenantId { get; set; }
    }
}
