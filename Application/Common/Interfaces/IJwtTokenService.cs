using Domain.Entities;

namespace Application.Common.Interfaces;

/// <summary>
/// Generates JWT tokens for authenticated users.
/// </summary>
public interface IJwtTokenService
{
    string GenerateToken(AppUser user, string tenantId, IList<string> roles);
}