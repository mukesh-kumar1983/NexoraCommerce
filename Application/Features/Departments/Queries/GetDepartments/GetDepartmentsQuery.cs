using Application.Features.Departments.Dtos;
using MediatR;

namespace Application.Features.Departments.Queries;

public class GetDepartmentsQuery : IRequest<ApiResponse<List<DepartmentDto>>>
{
}