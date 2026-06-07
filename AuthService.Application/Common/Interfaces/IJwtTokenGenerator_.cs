using NexoraEnterprise.AuthService.Domain;

namespace NexoraEnterprise.AuthService.Application.Common.Interfaces;

public interface IJwtTokenGenerator_
{
    string GenerateToken(AppUser user, IEnumerable<string> roles);

    string GenerateRefreshToken();
}