using MediatR;

namespace Application.Features.Departments.Commands;

public class CreateDepartmentCommand : IRequest<ApiResponse<Guid>>
{
    public string Title { get; set; } = string.Empty;
    public Guid TenantId { get; set; }
}