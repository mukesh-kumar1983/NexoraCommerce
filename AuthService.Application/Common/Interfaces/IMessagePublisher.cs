namespace NexoraEnterprise.AuthService.Application.Common.Interfaces;

public interface IMessagePublisher
{
    void Publish<T>(T message);
}