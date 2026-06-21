using MediatR;
using Application.Features.JobTitles.Queries.Models;

namespace Application.Features.JobTitles.Queries;

public class GetJobTitleByIdQuery : IRequest<ApiResponse<JobTitleDto?>>
{
    public Guid Id { get; set; }

    public GetJobTitleByIdQuery(Guid id)
    {
        Id = id;
    }
}