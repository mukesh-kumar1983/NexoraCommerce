using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthService.Application.Features.Lookup.JobTitles.DTOs
{
    public class JobTitleDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = default!;
    }
}
