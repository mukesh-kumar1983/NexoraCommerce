using Application.Common.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Infrastructure.Services;

/// <summary>
/// Implementation of current user context using HTTP pipeline.
/// </summary>
public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public Guid UserId =>
        Guid.Parse(_httpContextAccessor.HttpContext!
            .User.FindFirst("userId")!.Value);

    public Guid TenantId =>
        Guid.Parse(_httpContextAccessor.HttpContext!
            .Items["TenantId"]!.ToString()!);

    public string? Email =>
        _httpContextAccessor.HttpContext!
            .User.FindFirst(ClaimTypes.Email)?.Value;
}