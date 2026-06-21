using MediatR;
using Application.Features.Auth.DTOs;

namespace Application.Features.Auth.Commands.Login;

public class LoginCommand : IRequest<LoginResponse>
{
    public string TenantCode { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string Password { get; set; } = default!;
}