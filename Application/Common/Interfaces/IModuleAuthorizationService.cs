namespace Application.Common.Interfaces;

public interface IModuleAuthorizationService
{
    bool HasModuleAccess(string moduleCode);
    bool CanAccess(string moduleCode, string permission);
}