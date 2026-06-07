using MediatR;
using NexoraEnterprise.AuthService.Application.Features.Users.DTOs;

namespace NexoraEnterprise.AuthService.Application.Features.Users.Queries;

public class GetMyProfileQuery : IRequest<EmployeeDto> { }