using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NexoraEnterprise.SharedKernel.Common.Errors;

namespace Application.Features.Auth.Commands.Login;

/// <summary>
/// Handles user login for SaaS authentication system.
/// Flow: Tenant → User → Password → Roles → JWT Token
/// </summary>
public class LoginCommandHandler : IRequestHandler<LoginCommand, LoginResponse>
{
    private readonly IJwtTokenService _jwtTokenService;
    private readonly ITenantResolverService _tenantResolverService;
    private readonly UserManager<AppUser> _userManager;

    public LoginCommandHandler(
        IJwtTokenService jwtTokenService,
        ITenantResolverService tenantResolverService,
        UserManager<AppUser> userManager)
    {
        _jwtTokenService = jwtTokenService;
        _tenantResolverService = tenantResolverService;
        _userManager = userManager;
    }

    /// <summary>
    /// Executes login request with strict tenant isolation.
    /// </summary>
    public async Task<LoginResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        // 1. Resolve tenant FIRST (SaaS boundary enforcement)
        var tenant = await _tenantResolverService.ResolveAsync(request.TenantCode);

        if (tenant == null || !tenant.IsActive)
            throw new ApplicationException(ErrorCodes.Tenant_NotFound);

        // 2. Find user inside tenant scope (critical isolation rule)
        var user = await _userManager.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(x =>
                x.Email == request.Email &&
                x.TenantId == tenant.Id,
                cancellationToken);

        if (user == null)
            throw new ApplicationException(ErrorCodes.Auth_UserNotFound);

        // 3. Validate password
        var passwordValid = await _userManager.CheckPasswordAsync(user, request.Password);

        if (!passwordValid)
            throw new ApplicationException(ErrorCodes.Auth_InvalidCredentials);

        // 4. Get roles
        var roles = await _userManager.GetRolesAsync(user);

        // 5. Generate JWT (tenant-bound token)
        var token = _jwtTokenService.GenerateToken(
            user,
            tenant.Id.ToString(),
            roles);

        return new LoginResponse
        {
            Token = token
        };
    }
}