using MediatR;

namespace NexoraEnterprise.AuthService.Application.Features.Commands;
public class RegisterUserCommand : IRequest<Guid>
{
    public string Email { get; set; } = default!;
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public string Password { get; set; } = default!;
}
