using MediatR;

namespace NexoraEnterprise.AuthService.Application.Features.Users.Commands.CreateEmployeeCommand
{
    public class CreateEmployeeCommand : IRequest<int>
    {
        public string FirstName { get; set; } = default!;
        public string LastName { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string Department { get; set; } = default!;
    }
}
