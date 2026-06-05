using AuthService.Application.Features.Users.DTOs;
using MediatR;
using SharedKernel.Common;
using SharedKernel.Common.Models;

public class GetEmployeesPagedQuery
    : GridQuery,
      IRequest<PagedResult<EmployeeDto>>
{
}