using NexoraEnterprise.AuthService.Domain;

namespace AuthService.Application.Common.Interfaces;

public interface IUserRepository
{
    // Retrieval
    Task<AppUser?> GetByIdAsync(Guid id, CancellationToken ct = default);

    Task<UserProfile?> GetProfileByIdAsync(Guid id, CancellationToken ct = default);
    Task<AppUser?> GetByEmailAsync(string email, CancellationToken ct = default);
    Task<IEnumerable<AppUser>> GetAllAsync(CancellationToken ct = default);

    // Checks
    Task<bool> ExistsByEmailAsync(string email, CancellationToken ct = default);

    // Commands
    void Add(AppUser user);
    void Update(AppUser user);
    void Delete(AppUser user);
}