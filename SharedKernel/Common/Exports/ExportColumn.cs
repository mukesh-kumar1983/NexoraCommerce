using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedKernel.Common.Exports
{
    public class ExportColumn
    {
        public string Field { get; set; } = default!;   // property name

        public string Header { get; set; } = default!;  // column title

        public bool Ignore { get; set; } = false;       // skip column

        public int Order { get; set; }                  // column ordering
    }
}
