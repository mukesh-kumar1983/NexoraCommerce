namespace AuthService.Application.Common.Interfaces;

public interface IMessagePublisher
{
    void Publish<T>(T message);
}