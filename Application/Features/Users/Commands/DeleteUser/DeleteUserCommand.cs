using MediatR;

namespace Application.Features.Users.Commands.DeleteUser;

public class DeleteUserCommand : IRequest<ApiResponse>
{
    public Guid UserId { get; set; }
}