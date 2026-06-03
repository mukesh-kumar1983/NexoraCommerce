using MediatR;
using Microsoft.EntityFrameworkCore;
using AuthService.Application.Common.Interfaces;
using AuthService.Application.Features.Users.DTOs;

namespace AuthService.Application.Features.Users.Queries.GetUsers;

public class GetEmployeesQueryHandler : IRequestHandler<GetEmployeesQuery, List<EmployeeDto>>
{
    private readonly IAuthDbContext _context;
    private readonly ICurrentTenantService _tenant;

    public GetEmployeesQueryHandler(IAuthDbContext context, ICurrentTenantService tenant)
    {
        _context = context;
        _tenant = tenant;
    }

    public async Task<List<EmployeeDto>> Handle(GetEmployeesQuery request, CancellationToken cancellationToken)
    {
        var tenantId = _tenant.TenantId;

        return await (
            from u in _context.Users
            join p in _context.UserProfile on u.Id equals p.Id
            join d in _context.Department on p.DepartmentId equals d.Id
            join j in _context.JobTitle on p.JobTitleId equals j.Id
            where u.TenantId == tenantId && u.IsDeleted == false
            select new EmployeeDto
            {
                Id = u.Id,
                Email = u.Email,

                FirstName = p.FirstName ?? string.Empty,
                LastName = p.LastName ?? string.Empty,

                DepartmentId = Guid.Parse(p.DepartmentId.ToString()),
                DepartmentName = d.Title,

                JobTitleName = j.Title,
                JobTitleId = Guid.Parse(p.JobTitleId.ToString()),

                PhoneNumber = p.PhoneNumber,
                Address = p.Address,
                City = p.City,
                Country = p.Country,
                Gender = p.Gender,

                ProfileImageUrl = p.ProfileImageUrl
            }
                ).OrderBy(x => x.FirstName)
        .           ThenBy(x => x.LastName).ToListAsync(cancellationToken);
    }
}