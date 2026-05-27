using MediatR;
using AuthService.Application.Features.Lookup.Departments.DTOs;

namespace AuthService.Application.Features.Lookup.Departments.Queries;

public class GetDepartmentsQuery : IRequest<List<DepartmentDto>>
{
}