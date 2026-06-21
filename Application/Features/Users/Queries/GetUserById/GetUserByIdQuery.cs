using Application.Features.Users.DTOs;
using MediatR;

namespace Application.Features.Users.Queries.GetUserById;

public class GetUserByIdQuery : IRequest<ApiResponse<UserDto?>>
{
    public Guid UserId { get; set; }
}