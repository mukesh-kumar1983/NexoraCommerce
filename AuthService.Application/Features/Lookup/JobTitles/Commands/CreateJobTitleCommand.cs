using MediatR;

namespace NexoraEnterprise.AuthService.Application.Features.Lookup.JobTitles.Commands
{
    public class CreateJobTitleCommand : IRequest<Guid>
    {
        public string Title { get; set; } = default!;
    }
}
