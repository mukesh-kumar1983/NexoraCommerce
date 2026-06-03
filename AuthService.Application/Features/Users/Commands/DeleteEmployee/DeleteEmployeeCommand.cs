using MediatR;

namespace AuthService.Application.Features.Users.Commands.DeleteEmployee
{
    public class DeleteEmployeeCommand : IRequest<bool>
    {
        public Guid Id { get; set; }
    }
}
