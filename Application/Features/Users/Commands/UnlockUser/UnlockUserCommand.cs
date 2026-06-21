using MediatR;

namespace Application.Features.Users.Commands.UnlockUser;

public class UnlockUserCommand : IRequest<ApiResponse>
{
    public Guid UserId { get; set; }
}