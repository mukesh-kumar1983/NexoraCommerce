using NexoraEnterprise.AuthService.Domain;
using NexoraEnterprise.AuthService.Domain.Entities;

namespace NexoraEnterprise.AuthService.Application.Common.Interfaces
{
    /// <summary>
    /// Contract for JWT generation (implemented in Infrastructure)
    /// </summary>
    public interface IJwtTokenService
    {
        string GenerateToken(AppUser user, UserProfile profile, List<string> roles);
    }
}