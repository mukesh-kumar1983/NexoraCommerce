namespace NexoraEnterprise.AuthService.Application.Common.Interfaces;

public interface ICurrentTenantService
{
    Guid TenantId { get; }
}   