using AuthService.Application.Common.Interfaces;
using AuthService.Application.Features.Users.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AuthService.Application.Features.Users.Queries.GetUserById;

public class GetUEmployeeByIdQueryHandler : IRequestHandler<GetUserByIdQuery, EmployeeDto>
{
    private readonly IAuthDbContext _context;

    public GetUEmployeeByIdQueryHandler(IAuthDbContext context)
    {
        _context = context;
    }

    public async Task<EmployeeDto> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        var result = await (
            from u in _context.Users
            join p in _context.UserProfile on u.Id equals p.Id
            where u.Id == request.Id
            select new EmployeeDto
            {
                Id = u.Id,
                Email = u.Email,
                FirstName = p.FirstName!,
                LastName = p.LastName!,
                DepartmentId = Guid.Parse(p.DepartmentId.ToString()),
                JobTitleId = Guid.Parse(p.JobTitleId.ToString()),
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