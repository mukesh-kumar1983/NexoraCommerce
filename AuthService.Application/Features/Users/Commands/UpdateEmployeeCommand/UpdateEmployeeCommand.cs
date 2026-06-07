using MediatR;

namespace NexoraEnterprise.AuthService.Application.Features.Users.Commands.UpdateEmployeeCommand
{
    public class UpdateEmployeeCommand : IRequest<bool>
    {
        public Guid Id { get; set; }

        public string FirstName { get; set; } = default!;
        public string LastName { get; set; } = default!;
        public string Email { get; set; } = default!;

        public string? PhoneNumber { get; set; }
        public string? Address { get; set; }
        public string? City { get; set; }
        public string? Country { get; set; }
        public string? Gender { get; set; }

        public Guid DepartmentId { get; set; }
        public Guid JobTitleId { get; set; }

        public string? ProfileImageUrl { get; set; }
    }
}
