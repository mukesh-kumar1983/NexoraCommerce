using MediatR;
using Application.Common.Interfaces;

namespace Application.Features.JobTitles.Commands.CreateJobTitle;

public class CreateJobTitleCommand : IRequest<ApiResponse<Guid>>
{
    public string Title { get; set; } = string.Empty;
}