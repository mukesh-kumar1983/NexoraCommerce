using MediatR;
using Application.Common.Interfaces;
using Application.Features.Auth.DTOs;
using Domain.Entities;

namespace Application.Features.Auth.Commands.Login;

public class LoginCommandHandler : IRequestHandler<LoginCommand, AuthResponse>
{
    private readonly IIdentityService _identityService;
    private readonly IJwtTokenService _jwtTokenService;

    public LoginCommandHandler(
        IIdentityService identityService,
        IJwtTokenService jwtTokenService)
    {
        _identityService = identityService;
        _jwtTokenService = jwtTokenService;
    }

    public async Task<AuthResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await _identityService.FindByEmailAsync(request.Email);

        if (user == null)
            throw new Exception("Invalid credentials");

        var passwordValid = await _identityService.CheckPasswordAsync(user, request.Password);

        if (!passwordValid)
            throw new Exception("Invalid credentials");

        var tokenResult = await _jwtTokenService.GenerateTokenAsync(user);

        return new AuthResponse
        {
            AccessToken = tokenResult.Token,
            ExpiresAt = tokenResult.ExpiresAt,
            UserId = user.Id,
            TenantId = user.TenantId,
            Roles = tokenResult.Roles
        };
    }
}