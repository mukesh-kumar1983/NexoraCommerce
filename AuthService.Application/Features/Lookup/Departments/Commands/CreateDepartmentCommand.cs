using MediatR;

namespace AuthService.Application.Features.Lookup.Departments.Commands;

public class CreateDepartmentCommand : IRequest<Guid>
{
    public string Title { get; set; } = default!;
}