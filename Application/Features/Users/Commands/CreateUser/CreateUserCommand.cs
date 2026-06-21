using MediatR;

namespace AuthService.Application.Features.Users.Commands.CreateUser;

public class CreateUserCommand : IRequest<ApiResponse<Guid>>
{
    // Identity
    public string Email { get; set; } = default!;
    public string Password { get; set; } = default!;

    // Profile
    public string? FirstName { get; set; }
    public string? LastName { get; set; }

    public string? PhoneNumber { get; set; }

    // Organization
    public Guid? DepartmentId { get; set; }
    public Guid? JobTitleId { get; set; }

    // Security / Access
    public string Role { get; set; } = default!;
}