namespace NexoraEnterprise.AuthService.Application.Common.Interfaces
{
    public interface IPasswordHasher
    {
        bool Verify(string password, string hash);
        string Hash(string password);
    }
}