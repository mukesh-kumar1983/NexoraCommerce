using MediatR;

namespace Application.Features.JobTitles.Commands.DeleteJobTitle;

public class DeleteJobTitleCommand : IRequest<ApiResponse>
{
    public Guid Id { get; set; }
}