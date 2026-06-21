using Application.Common.Interfaces;
using Application.Features.Users.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Users.Queries.GetUserById;

public class GetUserByIdQueryHandler123
    : IRequestHandler<GetUserByIdQuery, ApiResponse<UserDto?>>
{
    private readonly IAuthDbContext _context;
    private readonly ICurrentTenantService _currentTenant;

    public GetUserByIdQueryHandler123(
        IAuthDbContext context,
        ICurrentTenantService currentTenant)
    {
        _context = context;
        _currentTenant = currentTenant;
    }

    public async Task<ApiResponse<UserDto?>> Handle(
        GetUserByIdQuery request,
        CancellationToken cancellationToken)
    {
        var tenantId = _currentTenant.TenantId;

        var result = await _context.UserProfiles
            .AsNoTracking()
            .Where(x => x.UserId == request.UserId && x.TenantId == tenantId)
            .Select(profile => new UserDto
            {
                UserId = profile.UserId,

                // Identity (join AppUser)
                Email = profile.User.Email,

                // Profile
                //FullName = profile.FirstName + " " + profile.LastName,
                PhoneNumber = profile.PhoneNumber,
                Gender = profile.Gender,
                ProfileImageUrl = profile.ProfileImageUrl,

                // Employment
                DepartmentId = profile.DepartmentId,
                DepartmentName = profile.Department != null ? profile.Department.Title : null,

                JobTitleId = profile.JobTitleId,
                JobTitleName = profile.JobTitle != null ? profile.JobTitle.Title : null,

                EmploymentStatus = profile.EmploymentStatus,

                // Security (from AppUser)
                IsLocked = profile.User.LockoutEnd != null &&
                           profile.User.LockoutEnd > DateTime.UtcNow,

                LockoutEnd = profile.User.LockoutEnd,

                Roles = profile.User.UserRoles
                    .Select(r => r.Role.Name)
                    .ToList()
            })
            .FirstOrDefaultAsync(cancellationToken);

        if (result == null)
        {
            return ApiResponse<UserDto?>.FailureResponse(
                "UserNotFound",
                new List<string> { "User not found" },
                "User not found"
            );
        }

        return ApiResponse<UserDto?>.SuccessResponse(
            result,
            "User retrieved successfully"
        );
    }
}