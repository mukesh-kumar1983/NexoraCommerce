using Domain.Entities;

namespace Application.Common.Interfaces;

public interface IJwtTokenService
{
    Task<(string Token, DateTime ExpiresAt, List<string> Roles)> GenerateTokenAsync(AppUser user);
}