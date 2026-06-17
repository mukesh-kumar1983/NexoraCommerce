using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using NexoraEnterprise.SharedKernel.Common.Models;

public class Repository<T> : IRepository<T>
    where T : BaseEntity
{
    protected readonly AuthDbContext _context;
    protected readonly DbSet<T> _dbSet;

    public Repository(AuthDbContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();
    }

    public async Task<T?> GetByIdAsync(Guid id)
    {
        return await _dbSet
            .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);
    }

    public async Task<List<T>> GetAllAsync()
    {
        return await _dbSet
            .Where(x => !x.IsDeleted)
            .ToListAsync();
    }

    public async Task AddAsync(T entity)
    {
        await _dbSet.AddAsync(entity);
    }

    public void Update(T entity)
    {
        _dbSet.Update(entity);
    }

    public void Delete(T entity)
    {
        // ❌ DO NOT physically delete in SaaS systems
        entity.IsDeleted = true;
        entity.DeletedAt = DateTime.UtcNow;

        _dbSet.Update(entity);
    }

    public async Task<bool> ExistsAsync(Guid id)
    {
        return await _dbSet.AnyAsync(x => x.Id == id && !x.IsDeleted);
    }
}