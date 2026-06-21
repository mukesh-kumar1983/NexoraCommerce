using Application.Common.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Domain.Entities;

namespace AuthService.Application.Features.Users.Commands.CreateUser;

public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, ApiResponse<Guid>>
{
    private readonly UserManager<AppUser> _userManager;
    private readonly IAuthDbContext _dbContext;
    private readonly ICurrentTenantService _currentTenant;

    public CreateUserCommandHandler(
        UserManager<AppUser> userManager,
        IAuthDbContext dbContext,
        ICurrentTenantService currentTenant)
    {
        _userManager = userManager;
        _dbContext = dbContext;
        _currentTenant = currentTenant;
    }

    public async Task<ApiResponse<Guid>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        if (_currentTenant.TenantId == Guid.Empty &&
    !_currentTenant.IsSuperAdmin)
        {
            return ApiResponse<Guid>.FailureResponse(
                "TenantMissing",
                new List<string> { "Tenant is missing." },
                "Tenant is missing."
            );
        }

        var tenantId = _currentTenant.TenantId;

        // ----------------------------------------------------
        // 1. CHECK DUPLICATE USER
        // ----------------------------------------------------
        var existingUser = await _userManager.Users
            .AnyAsync(x => x.Email == request.Email && x.TenantId == tenantId, cancellationToken);

        if (existingUser)
            return ApiResponse<Guid>.FailureResponse(
                "UserAlreadyExists",
                new List<string> { "User already exists for this tenant." },
                "User already exists for this tenant."
            );

        // ----------------------------------------------------
        // 2. CREATE IDENTITY USER
        // ----------------------------------------------------
        var user = new AppUser
        {
            UserName = request.Email,
            Email = request.Email,
            TenantId = tenantId,
            IsActive = true
        };

        var result = await _userManager.CreateAsync(user, request.Password);

        if (!result.Succeeded)
        {
            throw new Exception(string.Join(", ", result.Errors.Select(x => x.Description)));
        }

        // ----------------------------------------------------
        // 3. ASSIGN ROLE
        // ----------------------------------------------------
        if (!string.IsNullOrWhiteSpace(request.Role))
        {
            await _userManager.AddToRoleAsync(user, request.Role);
        }

        // ----------------------------------------------------
        // 4. CREATE USER PROFILE
        // ----------------------------------------------------
        var profile = new UserProfile
        {
            Id = user.Id,
            FirstName = request.FirstName,
            LastName = request.LastName,
            DepartmentId = request.DepartmentId,
            JobTitleId = request.JobTitleId,
            TenantId = tenantId
        };

        await _dbContext.UserProfiles.AddAsync(profile, cancellationToken);

        // ----------------------------------------------------
        // 5. SAVE CHANGES
        // ----------------------------------------------------
        await _dbContext.SaveChangesAsync(cancellationToken);

        return ApiResponse<Guid>.SuccessResponse(user.Id, "User created successfully");
    }
}