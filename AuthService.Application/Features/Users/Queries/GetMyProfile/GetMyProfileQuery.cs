using AuthService.Application.Features.Users.DTOs;
using MediatR;

public class GetMyProfileQuery : IRequest<UserDto> { }