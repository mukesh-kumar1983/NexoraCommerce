using Application.Common.Interfaces;
using Application.Features.Users.DTOs;
using Application.Features.Users.Queries.GetUserById;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace NexoraEnterprise.AuthService.Application.Features.Users.Queries.GetUserById
{
    public class GetUserByIdQueryHandler
    : IRequestHandler<GetUserByIdQuery, ApiResponse<UserDto?>>
    {
        private readonly IAuthDbContext _context;
        private readonly ICurrentTenantService _currentTenant;

        public GetUserByIdQueryHandler(
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
                .Where(x => x.UserId == request.UserId &&
                            x.TenantId == tenantId)
                .Select(profile => new UserDto
                {
                    UserId = profile.UserId,

                    // Identity
                    Email = profile.User.Email ?? string.Empty,

                    // Profile
                   // FullName = $"{profile.FirstName} {profile.LastName}".Trim(),
                    PhoneNumber = profile.PhoneNumber,
                    Gender = profile.Gender,
                    ProfileImageUrl = profile.ProfileImageUrl,

                    // Employment
                    DepartmentId = profile.DepartmentId,
                    DepartmentName = profile.Department != null
                        ? profile.Department.Title
                        : null,

                    JobTitleId = profile.JobTitleId,
                    JobTitleName = profile.JobTitle != null
                        ? profile.JobTitle.Title
                        : null,

                    EmploymentStatus = profile.EmploymentStatus,

                    // Security
                    IsLocked = profile.User.LockoutEnd.HasValue &&
                               profile.User.LockoutEnd > DateTimeOffset.UtcNow,

                    LockoutEnd = profile.User.LockoutEnd,

                    Roles = profile.User.UserRoles
                        .Where(ur => ur.Role != null && ur.Role.Name != null)
                        .Select(ur => ur.Role.Name!)
                        .ToList()
                })
                .FirstOrDefaultAsync(cancellationToken);

            if (result == null)
            {
                return ApiResponse<UserDto?>.FailureResponse(
                    "UserNotFound",
                    new List<string> { "User not found" },
                    "User not found");
            }

            return ApiResponse<UserDto?>.SuccessResponse(
                result,
                "User retrieved successfully");
        }
    }
}
