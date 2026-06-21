using MediatR;

namespace Application.Features.Departments.Commands;

public class UpdateDepartmentCommand : IRequest<ApiResponse>
{

    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public Guid TenantId { get; set; }
}