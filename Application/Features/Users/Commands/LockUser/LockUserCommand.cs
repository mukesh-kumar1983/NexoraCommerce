using MediatR;

namespace Application.Features.Users.Commands.LockUser;

public class LockUserCommand : IRequest<ApiResponse>
{
    public Guid UserId { get; set; }
    public int LockMinutes { get; set; } = 60; // default 1 hour
}