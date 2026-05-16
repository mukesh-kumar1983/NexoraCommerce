using AuthService.Application.Common.Interfaces;
using RabbitMQ.Client;
using Serilog;
using System.Text;
using System.Text.Json;

namespace AuthService.Infrastructure.Messaging;

/// <summary>
/// RabbitMQ publisher responsible for publishing integration events
/// from AuthService to the messaging broker.
///
/// Responsibilities:
/// ------------------------------------------------------------
/// - Serialize integration events into JSON
/// - Publish messages to RabbitMQ queues
/// - Mark messages as persistent for durability
///
/// Important:
/// ------------------------------------------------------------
/// This class DOES NOT create or manage queues.
/// Queue ownership belongs to the consumer service
/// (EmailService.Worker).
///
/// Architecture Flow:
/// ------------------------------------------------------------
/// AuthService → RabbitMQ → EmailService.Worker
/// </summary>
public class RabbitMqPublisher : IMessagePublisher, IDisposable
{
    private readonly IConnection _connection;
    private readonly IModel _channel;

    private const string QueueName = "user.registered.queue";

    public RabbitMqPublisher()
    {
        var factory = new ConnectionFactory()
        {
            HostName = "localhost",
            Port = 5672,
            UserName = "admin",
            Password = "admin123"
        };

        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();

        Log.Information(
            "RabbitMQ publisher initialized for queue {Queue}",
            QueueName
        );
    }

    /// <summary>
    /// Publishes an integration event to RabbitMQ.
    /// </summary>
    /// <typeparam name="T">Type of message/event</typeparam>
    /// <param name="message">Event payload</param>
    public void Publish<T>(T message)
    {
        var json = JsonSerializer.Serialize(message);
        var body = Encoding.UTF8.GetBytes(json);

        var properties = _channel.CreateBasicProperties();

        // Message survives broker restart
        properties.Persistent = true;

        _channel.BasicPublish(
            exchange: "",
            routingKey: QueueName,
            basicProperties: properties,
            body: body
        );

        Log.Information(
            "Message published successfully to queue {Queue}",
            QueueName
        );
    }

    /// <summary>
    /// Cleans up RabbitMQ resources.
    /// </summary>
    public void Dispose()
    {
        _channel?.Close();
        _connection?.Close();

        _channel?.Dispose();
        _connection?.Dispose();

        Log.Information("RabbitMQ publisher disposed");
    }
}