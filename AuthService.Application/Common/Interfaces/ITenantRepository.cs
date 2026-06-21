using NexoraEnterprise.AuthService.Domain.Entities;

namespace Application.Common.Interfaces;

public interface ITenantRepository
{
    Task<Tenant?> GetByIdAsync(Guid id);
}