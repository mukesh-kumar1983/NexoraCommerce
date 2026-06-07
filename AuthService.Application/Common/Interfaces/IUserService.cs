namespace NexoraEnterprise.AuthService.Application.Common.Interfaces
{
    public interface IUserService
    {
        Task UpdateProfileImageAsync(string userId, string imageUrl);
    }
}
