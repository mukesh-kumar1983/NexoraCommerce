using MediatR;

namespace Application.Features.Users.Commands.UpdateUser;

public class UpdateUserCommand : IRequest<ApiResponse>
{
    public Guid UserId { get; set; }

    public string FullName { get; set; } = string.Empty;
    public string? PhoneNumber { get; set; }

    public Guid? DepartmentId { get; set; }
    public Guid? JobTitleId { get; set; }
}