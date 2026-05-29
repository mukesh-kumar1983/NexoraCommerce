using AuthService.Application.Common.Interfaces;
using AuthService.Application.Features.Users.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AuthService.Application.Features.Users.Queries.GetUserById;

public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, UserDto>
{
    private readonly IAuthDbContext _context;

    public GetUserByIdQueryHandler(IAuthDbContext context)
    {
        _context = context;
    }

    public async Task<UserDto> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        var result = await (
            from u in _context.Users
            join p in _context.UserProfile on u.Id equals p.Id
            where u.Id == request.Id
            select new UserDto
            {
                Id = u.Id,
                Email = u.Email,
                FirstName = p.FirstName!,
                LastName = p.LastName!,
                DepartmentId = p.DepartmentId,
                JobTitleId = p.JobTitleId,
                PhoneNumber = p.PhoneNumber,
                Address = p.Address,
                City = p.City,
                Country = p.Country,
                Gender = p.Gender,
                ProfileImageUrl = p.ProfileImageUrl
            }
        ).FirstOrDefaultAsync(cancellationToken);

        return result!;
    }
}