using MediatR;

namespace Application.Features.Departments.Commands;

public class DeleteDepartmentCommand : IRequest<ApiResponse>
{
    public Guid Id { get; set; }

    public DeleteDepartmentCommand(Guid id)
    {
        Id = id;
    }
}