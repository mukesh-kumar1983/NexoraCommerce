namespace NexoraEnterprise.AuthService.Application.Features.Lookup.JobTitles.Queries
{
    using AuthService.Application.Features.Lookup.JobTitles.DTOs;
    using MediatR;

    public class GetJobTitlesQuery : IRequest<List<JobTitleDto>>
    {
    }
}
