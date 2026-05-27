using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthService.Application.Features.Lookup.JobTitles.Commands
{
    public class CreateJobTitleCommand : IRequest<Guid>
    {
        public string Title { get; set; } = default!;
    }
}
