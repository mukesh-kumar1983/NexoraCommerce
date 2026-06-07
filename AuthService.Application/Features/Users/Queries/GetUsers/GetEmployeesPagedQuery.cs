using NexoraEnterprise.AuthService.Application.Features.Users.DTOs;
using MediatR;
using NexoraEnterprise.SharedKernel.Common.Models;
using SharedKernel.Common.Models;

public class GetEmployeesPagedQuery
    : GridQuery,
      IRequest<PagedResult<EmployeeDto>>
{
}