using MediatR;
using NexoraEnterprise.AuthService.Application.Features.Users.DTOs;

namespace NexoraEnterprise.AuthService.Application.Features.Users.Queries.GetUsers;

public class GetEmployeesQuery : IRequest<List<EmployeeDto>>
{
}