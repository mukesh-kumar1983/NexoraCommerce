using AuthService.Domain.Entities;

namespace AuthService.Application.Common.Interfaces;

public interface IJwtTokenGenerator_
{
    string GenerateToken(AppUser user, IEnumerable<string> roles);

    string GenerateRefreshToken();
}