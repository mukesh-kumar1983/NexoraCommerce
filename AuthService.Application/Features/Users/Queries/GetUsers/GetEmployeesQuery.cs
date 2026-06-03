using MediatR;
using AuthService.Application.Features.Users.DTOs;

namespace AuthService.Application.Features.Users.Queries.GetUsers;

public class GetEmployeesQuery : IRequest<List<EmployeeDto>>
{
}