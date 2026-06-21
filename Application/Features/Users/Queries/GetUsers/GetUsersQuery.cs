using Application.Features.Users.DTOs;
using MediatR;

namespace Application.Features.Users.Queries.GetUsers
{
    public class GetUsersQuery : IRequest<ApiResponse<List<UserDto>>>
    {
    }
}
