using Domain.Entities;

namespace Application.Common.Interfaces;

public interface IIdentityService
{
    Task<Guid> CreateUserAsync(
        string email,
        string password,
        Guid tenantId);

    Task AddToRoleAsync(
        Guid userId,
        string role);

    Task<AppUser?> FindByIdAsync(
        Guid userId);

    Task<AppUser?> FindByEmailAsync(
        string email);

    Task<bool> CheckPasswordAsync(
        AppUser user,
        string password);

    Task<IList<string>> GetRolesAsync(
        AppUser user);
}