using Application.Features.Departments.Dtos;
using MediatR;

namespace NexoraEnterprise.AuthService.Application.Features.Departments.Queries.GetDepartmentById
{
    public class GetDepartmentByIdQuery : IRequest<ApiResponse<DepartmentDto?>>
    {
        public Guid Id { get; set; }

        public GetDepartmentByIdQuery(Guid id)
        {
            Id = id;
        }
    }
}
