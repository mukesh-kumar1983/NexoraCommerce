using AuthService.Application.Features.Users.DTOs;
using MediatR;

public class GetUserByIdQuery : IRequest<UserDto>
{
    public Guid Id { get; set; }
}