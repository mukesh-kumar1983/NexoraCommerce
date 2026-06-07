using Microsoft.AspNetCore.Http;
using NexoraEnterprise.AuthService.Application.Common.Interfaces;
using System.Security.Claims;

namespace AuthService.Infrastructure.Repositories;

public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public Guid UserId =>
        Guid.TryParse(
            _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value,
            out var id)
            ? id
            : Guid.Empty;

    public string Email =>
        _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Email)?.Value
        ?? string.Empty;
}