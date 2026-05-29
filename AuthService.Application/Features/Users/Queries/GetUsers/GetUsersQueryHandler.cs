using MediatR;
using Microsoft.EntityFrameworkCore;
using AuthService.Application.Common.Interfaces;
using AuthService.Application.Features.Users.DTOs;

namespace AuthService.Application.Features.Users.Queries.GetUsers;

public class GetUsersQueryHandler : IRequestHandler<GetUsersQuery, List<UserDto>>
{
    private readonly IAuthDbContext _context;
    private readonly ICurrentTenantService _tenant;

    public GetUsersQueryHandler(IAuthDbContext context, ICurrentTenantService tenant)
    {
        _context = context;
        _tenant = tenant;
    }

    public async Task<List<UserDto>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
    {
        var tenantId = _tenant.TenantId;

        return await (
            from u in _context.Users
            join p in _context.UserProfile on u.Id equals p.Id
            join d in _context.Department on p.DepartmentId equals d.Id
            join j in _context.JobTitle on p.JobTitleId equals j.Id
            where u.TenantId == tenantId
            select new UserDto
            {
                Id = u.Id,
                Email = u.Email,

                FirstName = p.FirstName ?? string.Empty,
                LastName = p.LastName ?? string.Empty,

                DepartmentId = p.DepartmentId,
                Department = d.Title,

                JobTitle = j.Title,
                JobTitleId = p.JobTitleId,

                PhoneNumber = p.PhoneNumber,
                Address = p.Address,
                City = p.City,
                Country = p.Country,
                Gender = p.Gender,

                ProfileImageUrl = p.ProfileImageUrl
            }
        ).ToListAsync(cancellationToken);
    }
}