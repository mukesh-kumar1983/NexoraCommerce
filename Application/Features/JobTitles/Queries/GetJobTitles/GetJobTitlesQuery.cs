using MediatR;
using Application.Features.JobTitles.Queries.Models;

namespace Application.Features.JobTitles.Queries;

public class GetJobTitlesQuery : IRequest<ApiResponse<List<JobTitleDto>>>
{
}