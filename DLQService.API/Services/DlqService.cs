using DLQService.API.DTOs;
using RabbitMQ.Client;
using System.Text;

namespace DLQService.API.Services
{
    public class DlqService
    {
        private const string DlqQueue = "user.registered.dlq";
        private const string MainQueue = "user.registered.queue";

        private readonly IConnection _connection;
        private readonly IModel _channel;

        public DlqService()
        {
            var factory = new ConnectionFactory
            {
                HostName = "localhost",
                UserName = "admin",
                Password = "admin123"
            };

            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
        }

        public List<DlqMessageDto> GetMessages(int maxMessages = 20)
        {
            var messages = new List<DlqMessageDto>();

            for (int i = 0; i < maxMessages; i++)
            {
                var result = _channel.BasicGet(DlqQueue, autoAck: false);

                if (result == null)
                    break;

                var payload = Encoding.UTF8.GetString(result.Body.ToArray());

                messages.Add(new DlqMessageDto
                {
                    DeliveryTag = result.DeliveryTag,
                    Payload = payload,
                    RetrievedAt = DateTime.UtcNow
                });

                _channel.BasicNack(result.DeliveryTag, false, true);
            }

            return messages;
        }

        public void ReplayMessage(ulong deliveryTag, string payload)
        {
            var body = Encoding.UTF8.GetBytes(payload);

            _channel.BasicPublish(
                exchange: "",
                routingKey: MainQueue,
                basicProperties: null,
                body: body
            );

            _channel.BasicAck(deliveryTag, false);
        }

        public void DeleteMessage(ulong deliveryTag)
        {
            _channel.BasicAck(deliveryTag, false);
        }

    }
}
