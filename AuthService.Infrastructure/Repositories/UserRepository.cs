using AuthService.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;
using NexoraEnterprise.AuthService.Domain;
using NexoraEnterprise.AuthService.Infrastructure.Persistence;

namespace AuthService.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly AuthDbContext _context;

    public UserRepository(AuthDbContext context)
    {
        _context = context;
    }

    public async Task<AppUser?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        return await _context.Users
            .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
            .FirstOrDefaultAsync(u => u.Id == id, ct);
    }

    public async Task<UserProfile?> GetProfileByIdAsync(Guid id, CancellationToken ct = default)
    {
        return await _context.UserProfile
            .FirstOrDefaultAsync(u => u.Id == id, ct);
    }

    public async Task<AppUser?> GetByEmailAsync(string email, CancellationToken ct = default)
    {
        return await _context.Users
            .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
            .FirstOrDefaultAsync(u => u.Email == email, ct);
    }

    public async Task<IEnumerable<AppUser>> GetAllAsync(CancellationToken ct = default)
    {
        return await _context.Users
            .AsNoTracking()
            .ToListAsync(ct);
    }

    public async Task<bool> ExistsByEmailAsync(string email, CancellationToken ct = default)
    {
        return await _context.Users.AnyAsync(u => u.Email == email, ct);
    }

    public void Add(AppUser user)
    {
        _context.Users.Add(user);
    }

    public void Update(AppUser user)
    {
        // EF Core tracks changes automatically if the entity was fetched 
        // from the same context instance. This is for explicit updates.
        _context.Users.Update(user);
    }

    public void Delete(AppUser user)
    {
        _context.Users.Remove(user);
    }
}