namespace Application.Common.Interfaces;

public interface IModuleAccessService
{
    Task<bool> IsEnabledAsync(string moduleCode);
    Task EnsureEnabledAsync(string moduleCode);
}