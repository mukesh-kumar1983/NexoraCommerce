using MediatR;
using Microsoft.EntityFrameworkCore;
using NexoraEnterprise.AuthService.Application.Common.Interfaces;
using NexoraEnterprise.AuthService.Application.Features.Users.DTOs;

namespace NexoraEnterprise.AuthService.Application.Features.Users.Queries.GetMyProfile;

public class GetMyProfileQueryHandler : IRequestHandler<GetMyProfileQuery, EmployeeDto>
{
    private readonly IAuthDbContext _context;
    private readonly ICurrentUserService _currentUser;

    public GetMyProfileQueryHandler(
        IAuthDbContext context,
        ICurrentUserService currentUser)
    {
        _context = context;
        _currentUser = currentUser;
    }

    public async Task<EmployeeDto> Handle(GetMyProfileQuery request, CancellationToken cancellationToken)
    {
        var userId = _currentUser.UserId;

        var result = await (
            from u in _context.Users
            join p in _context.UserProfile on u.Id equals p.Id
            where u.Id == userId
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