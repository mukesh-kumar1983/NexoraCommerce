using AuthService.Application.Features.Authentication.DTOs;
using MediatR;
using SharedKernel.Common.Models;

namespace AuthService.Application.Features.Commands
{
    /// <summary>
    /// CQRS command for user login
    /// </summary>
    public sealed class LoginCommand
    : IRequest<ApiResponse<AuthResponseDto>>
    {
        public string Email { get; set; } = default!;
        public string Password { get; set; } = default!;
    }
}