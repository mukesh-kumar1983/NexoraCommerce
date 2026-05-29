using AuthService.Application.Common.Interfaces;
using AuthService.Application.Features.Users.Commands.UpdateUserProfile;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


public class UpdateUserProfileCommandHandler : IRequestHandler<UpdateUserProfileCommand, bool>
{
    private readonly IAuthDbContext _context;
    private readonly ICurrentTenantService _tenant;

    public UpdateUserProfileCommandHandler(
        IAuthDbContext context,
        ICurrentTenantService tenant)
    {
        _context = context;
        _tenant = tenant;
    }

    public async Task<bool> Handle([FromBody] UpdateUserProfileCommand request, CancellationToken cancellationToken)
    {
        var tenantId = _tenant.TenantId;

        // 1. Get User (tenant safe)
        var user = await _context.Users
            .FirstOrDefaultAsync(
                x => x.Id == request.UserId && x.TenantId == tenantId,
                cancellationToken);

        if (user == null)
            return false;

        // 2. Get Profile (add tenant safety if column exists)
        var profile = await _context.UserProfile
            .FirstOrDefaultAsync(
                x => x.Id == request.UserId,
                cancellationToken);

        if (profile == null)
            return false;

        // 3. Update User (auth-level info)
        user.FirstName = request.FirstName;
        user.LastName = request.LastName;

        // 4. Update Profile (extended info)
        profile.FirstName = request.FirstName;
        profile.LastName = request.LastName;

        profile.DepartmentId = request.DepartmentId;
        profile.JobTitleId = request.JobTitleId;
        profile.PhoneNumber = request.PhoneNumber;
        profile.Address = request.Address;
        profile.City = request.City;
        profile.Country = request.Country;
        profile.Gender = request.Gender;

        // 5. Profile image update (safe overwrite)
        if (!string.IsNullOrWhiteSpace(request.ProfileImageUrl))
        {
            profile.ProfileImageUrl = request.ProfileImageUrl;
        }

        // 6. Save
        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }
}