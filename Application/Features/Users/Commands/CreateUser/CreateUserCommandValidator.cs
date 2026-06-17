using FluentValidation;

namespace AuthService.Application.Features.Users.Commands.CreateUser;

public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
{
    public CreateUserCommandValidator()
    {
        // ---------------- EMAIL ----------------
        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage("Email is required")
            .EmailAddress()
            .WithMessage("Invalid email format");

        // ---------------- PASSWORD ----------------
        RuleFor(x => x.Password)
            .NotEmpty()
            .WithMessage("Password is required")
            .MinimumLength(8)
            .WithMessage("Password must be at least 8 characters")
            .Matches("[A-Z]")
            .WithMessage("Password must contain at least one uppercase letter")
            .Matches("[a-z]")
            .WithMessage("Password must contain at least one lowercase letter")
            .Matches("[0-9]")
            .WithMessage("Password must contain at least one number");

        // ---------------- ROLE ----------------
        RuleFor(x => x.Role)
            .NotEmpty()
            .WithMessage("Role is required");

        // ---------------- NAME (optional but safe validation) ----------------
        RuleFor(x => x.FirstName)
            .MaximumLength(50)
            .When(x => !string.IsNullOrEmpty(x.FirstName));

        RuleFor(x => x.LastName)
            .MaximumLength(50)
            .When(x => !string.IsNullOrEmpty(x.LastName));

        // ---------------- ORGANIZATION ----------------
        RuleFor(x => x.DepartmentId)
            .NotEmpty()
            .When(x => x.DepartmentId.HasValue)
            .WithMessage("Invalid Department");

        RuleFor(x => x.JobTitleId)
            .NotEmpty()
            .When(x => x.JobTitleId.HasValue)
            .WithMessage("Invalid Job Title");
    }
}