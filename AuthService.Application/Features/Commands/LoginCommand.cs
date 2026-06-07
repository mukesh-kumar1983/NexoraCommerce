using MediatR;
using NexoraEnterprise.AuthService.Application.Features.Authentication.DTOs;
using NexoraEnterprise.SharedKernel.Common.Models;

namespace NexoraEnterprise.AuthService.Application.Features.Commands
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