using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NexoraEnterprise.AuthService.Application.Common.Interfaces;
using NexoraEnterprise.AuthService.Domain;

namespace NexoraEnterprise.AuthService.Application;

public class UpdateMyProfileCommandHandler : IRequestHandler<UpdateMyProfileCommand, bool>
{
    private readonly IAuthDbContext _context;
    private readonly ICurrentTenantService _tenant;
    private readonly ICurrentUserService _currentUser;

    public UpdateMyProfileCommandHandler(
        IAuthDbContext context,
        ICurrentTenantService tenant,
        ICurrentUserService currentUser)
    {
        _context = context;
        _tenant = tenant;
        _currentUser = currentUser;
    }

    public async Task<bool> Handle([FromBody] UpdateMyProfileCommand request, CancellationToken cancellationToken)
    {
        var tenantId = _tenant.TenantId;

       // 1.Get User(tenant safe)
        var user = await _context.Users
            .FirstOrDefaultAsync(
                x => x.Id == _currentUser.UserId && x.TenantId == tenantId,
                cancellationToken);

        if (user == null)
            return false;

        // 2. Get Profile (FIXED RELATION)
        var profile = await _context.UserProfile
            .FirstOrDefaultAsync(
                x => x.Id == _currentUser.UserId,
                cancellationToken);

        if (profile == null)
        {
            // optional: auto-create profile (recommended in real systems)
            profile = new UserProfile
            {
                Id = _currentUser.UserId
            };

            _context.UserProfile.Add(profile);
        }

        

        //// 3. Update USER (identity-level)
        //user.FirstName = request.FirstName;
        //user.LastName = request.LastName;

        // 4. Update PROFILE (business-level)
        profile.FirstName = request.FirstName;
        profile.LastName = request.LastName;

        profile.DepartmentId = request.DepartmentId;
        profile.JobTitleId = request.JobTitleId;
        profile.PhoneNumber = request.PhoneNumber;
        profile.Address = request.Address;
        profile.City = request.City;
        profile.Country = request.Country;
        profile.Gender = request.Gender;

        // 5. Image update (safe overwrite)
        if (!string.IsNullOrWhiteSpace(request.ProfileImageUrl))
        {
            profile.ProfileImageUrl = request.ProfileImageUrl;
        }

        // 6. Save
        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }
}