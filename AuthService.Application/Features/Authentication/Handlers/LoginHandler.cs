using AuthService.Application.Common.Interfaces;
using MediatR;
using NexoraEnterprise.AuthService.Application.Common.Interfaces;
using NexoraEnterprise.AuthService.Application.Features.Authentication.DTOs;
using NexoraEnterprise.AuthService.Application.Features.Commands;
using NexoraEnterprise.SharedKernel.Common.Models;

namespace NexoraEnterprise.AuthService.Application.Features.Authentication.Commands.Login;

public class LoginCommandHandler
    : IRequestHandler<LoginCommand, ApiResponse<AuthResponseDto>>
{
    private readonly IJwtTokenService _jwt;
    private readonly IUserRepository _userRepository;

    public LoginCommandHandler(
        IJwtTokenService jwt,
        IUserRepository userRepository)
    {
        _jwt = jwt;
        _userRepository = userRepository;
    }

    public async Task<ApiResponse<AuthResponseDto>> Handle(
        LoginCommand request,
        CancellationToken cancellationToken)
    {
        // 1. Fetch user from DB
        var user = await _userRepository.GetByEmailAsync(request.Email);

       

        if (user == null)
        {
            return ApiResponse<AuthResponseDto>
                .FailureResponse("INVALID_CREDENTIALS", new[] { "Invalid credentials" }, "Invalid Credebtials");
        }

        var userProfile = await _userRepository.GetProfileByIdAsync(user.Id);

        if(userProfile.IsActive)
        {
            return ApiResponse<AuthResponseDto>
                    .FailureResponse( "IN_ACTIVE", new[] { "This user is currently inactive, please contact your admin" }, "This user is currently inactive, please contact your admin");
        }

        if (userProfile.IsDeleted)
        {
            return ApiResponse<AuthResponseDto>
                    .FailureResponse("DELETED", new[] { "This user is currently marked as deleted, please contact your admin", "This user is currently marked as deleted, please contact your admin" });
        }

        if (user.IsLocked)
        {
            return ApiResponse<AuthResponseDto>
                    .FailureResponse("LOCKED", new[] { "This user is currently marked as locked, please contact your admin", "This user is currently marked as locked, please contact your admin" });
        }

        // 2. Verify password (temporary logic - replace later with hashing)

        //var passwordValid = user.PasswordHash == request.Password;
        var passwordValid = BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash);

        if (!passwordValid)
        {
            return ApiResponse<AuthResponseDto>
                .FailureResponse("INVALID_PASSWORD", new[] { "Invalid Password" }, "Invalid Password");
        }

        // 3. Get roles from DB (or navigation property)
        var roles = user.UserRoles?.Select(r => r.Role.Name).ToList()
                    ?? new List<string> { "User" };

        // 4. Generate JWT using REAL user
        var token = _jwt.GenerateToken(user, userProfile!, roles);

        // 5. Build response
        var response = new AuthResponseDto
        {
            Token = token,
            Email = user.Email!,
            FirstName = userProfile.FirstName,
            LastName = userProfile.LastName,
            
            Roles = roles
        };

        return ApiResponse<AuthResponseDto>
            .SuccessResponse(response, "Login successful");
    }
}