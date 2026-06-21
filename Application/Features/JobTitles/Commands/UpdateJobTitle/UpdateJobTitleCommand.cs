using MediatR;

namespace Application.Features.JobTitles.Commands.UpdateJobTitle;

public class UpdateJobTitleCommand : IRequest<ApiResponse>
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
}