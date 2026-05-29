using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthService.Application.Features.Lookup.JobTitles.Queries
{
    using AuthService.Application.Features.Lookup.JobTitles.DTOs;
    using MediatR;

    public class GetJobTitlesQuery : IRequest<List<JobTitleDto>>
    {
    }
}
