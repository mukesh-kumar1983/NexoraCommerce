using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.UserProfiles.Commands.UpsertUserProfile;

/// <summary>
/// Handles creation or update of user profile in tenant context.
/// </summary>
public class UpsertUserProfileCommandHandler : IRequestHandler<UpsertUserProfileCommand, ApiResponse<Guid>>
{
    private readonly IAuthDbContext _context;
    private readonly ICurrentUserService _currentUser;

    public UpsertUserProfileCommandHandler(
        IAuthDbContext context,
        ICurrentUserService currentUser)
    {
        _context = context;
        _currentUser = currentUser;
    }

    public async Task<ApiResponse<Guid>> Handle(UpsertUserProfileCommand request, CancellationToken cancellationToken)
    {
        bool isNewProfile = false;
        if (_currentUser?.UserId == null)
            return ApiResponse<Guid>.FailureResponse("User context missing", new List<string> { "Authenticated user context is required" }, "Authenticated user context is required");

        if (_currentUser?.TenantId == null)
            return ApiResponse<Guid>.FailureResponse("Tenant context missing", new List<string> { "Tenant context is required" }, "Tenant context is required");

        var profile = await _context.UserProfiles
            .FirstOrDefaultAsync(x =>
                x.UserId == _currentUser.UserId &&
                x.TenantId == _currentUser.TenantId,
                cancellationToken);

        if (profile == null)
        {
            profile = new UserProfile
            {
                Id = Guid.NewGuid(),
                UserId = _currentUser.UserId,
                TenantId = _currentUser.TenantId,
                CreatedAt = DateTime.UtcNow
            };

            _context.UserProfiles.Add(profile);
            isNewProfile = true;
        }

        profile.FirstName = request.FirstName;
        profile.LastName = request.LastName;
        profile.PhoneNumber = request.PhoneNumber;
        profile.Address = request.Address;
        profile.City = request.City;
        profile.Country = request.Country;
        profile.Gender = request.Gender;
        profile.DateOfBirth = request.DateOfBirth;

        profile.DepartmentId = request.DepartmentId;
        profile.JobTitleId = request.JobTitleId;
        profile.ProfileImageUrl = request.ProfileImageUrl;

        profile.ModifiedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync(cancellationToken);

        if (isNewProfile)
        {
            return new ApiResponse<Guid>
            {
                Data = profile.Id,
                Success = true,
                Message = "User profile inserted successfully"
            };
        }
        else
        {
            return new ApiResponse<Guid>
            {
                Data = profile.Id,
                Success = true,
                Message = "User profile updated successfully"
            };
        }
    }
}