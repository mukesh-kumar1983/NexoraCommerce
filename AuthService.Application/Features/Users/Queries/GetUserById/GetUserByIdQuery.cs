using NexoraEnterprise.AuthService.Application.Features.Users.DTOs;
using MediatR;

public class GetUserByIdQuery : IRequest<EmployeeDto>
{
    public Guid Id { get; set; }
}