using MediatR;
using NexoraEnterprise.AuthService.Application.Features.Lookup.Departments.DTOs;

namespace NexoraEnterprise.AuthService.Application.Features.Lookup.Departments.Queries;

public class GetDepartmentsQuery : IRequest<List<DepartmentDto>>
{
}