namespace EmailService.Application.Common.Settings;

/// <summary>
/// Central RabbitMQ configuration for exchanges, queues, and DLQ setup.
/// This ensures no infrastructure values are hardcoded in services.
/// </summary>
public class RabbitMqSettings
{
    // Connection
    public string HostName { get; set; } = string.Empty;
    public int Port { get; set; } = 5672;
    public string UserName { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;

    // Main Messaging
    public string QueueName { get; set; } = string.Empty;

    // Dead Letter Queue
    public string DeadLetterQueue { get; set; } = string.Empty;

    // 🔥 Exchange Configuration (IMPORTANT PART)
    public string DeadLetterExchange { get; set; } = string.Empty;
    public string DeadLetterRoutingKey { get; set; } = string.Empty;
}