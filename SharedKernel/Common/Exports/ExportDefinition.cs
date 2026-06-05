using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedKernel.Common.Exports
{
    public class ExportDefinition
    {
        public List<ExportColumn> Columns { get; set; } = new();

        public string SheetName { get; set; } = "Sheet1";
    }
}
