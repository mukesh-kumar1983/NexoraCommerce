using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedKernel.Common.Exports
{
    public class ExportFileResult
    {
        public byte[] FileContent { get; set; } = default!;

        public string FileName { get; set; } = default!;

        public string ContentType { get; set; } = default!;
    }
}
