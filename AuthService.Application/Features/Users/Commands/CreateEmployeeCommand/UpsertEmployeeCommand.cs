using MediatR;
using NexoraEnterprise.SharedKernel.Common.Models;

namespace NexoraEnterprise.AuthService.Application.Features.Users.Commands;

public class UpsertEmployeeCommand : IRequest<ApiResponse<Guid>>
{
    public Guid? Id { get; set; } // null = create, not null = update

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

    //public Guid TenantId { get; set; }
}
