namespace AuthService.Application.Common.Interfaces;

public interface ICurrentTenantService
{
    Guid TenantId { get; }
}   