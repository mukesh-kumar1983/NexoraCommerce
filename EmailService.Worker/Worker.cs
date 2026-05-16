using EmailService.Application.Common.Settings;
using EmailService.Application.Interfaces;
using EmailService.Domain.Entities;
using EmailService.Worker.Processors;
using EmailService.Worker.Services;
using Microsoft.Extensions.Options;
using Polly;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Serilog;
using Serilog.Context;
using SharedContracts.Events;
using System.Text;
using System.Text.Json;

namespace EmailService.Worker;

public class Worker : BackgroundService
{
    // ========================= RABBITMQ =========================
    private IConnection? _connection;
    private IModel? _channel;

    // ========================= SERVICES =========================
    private readonly IEmailService _emailService;
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly RabbitMqSettings _rabbit;
    private readonly EmailRetryProcessor _retryProcessor;
    private readonly RetrySettings _retrySettings;

    // ========================= POLLY =========================
    private readonly IAsyncPolicy _emailRetryPolicy;

    public Worker(
        IEmailService emailService,
        IServiceScopeFactory scopeFactory,
        IOptions<RabbitMqSettings> rabbitOptions,
        EmailRetryProcessor retryProcessor,
        IOptions<RetrySettings> retryOptions)
    {
        _emailService = emailService;
        _scopeFactory = scopeFactory;
        _rabbit = rabbitOptions.Value;
        _retryProcessor = retryProcessor;
        _retrySettings = retryOptions.Value;

        _emailRetryPolicy = Policy
            .Handle<Exception>()
            .WaitAndRetryAsync(
                _retrySettings.MaxRetryCount,
                _ => TimeSpan.FromSeconds(_retrySettings.DelaySeconds),
                (ex, delay, retryCount, _) =>
                {
                    Log.Warning(
                        "Email retry {Retry}/{Max} after {Delay}s. Error: {Error}",
                        retryCount,
                        _retrySettings.MaxRetryCount,
                        delay.TotalSeconds,
                        ex.Message);
                });
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var queueName = _rabbit.QueueName;
        var dlqName = _rabbit.DeadLetterQueue;

        // ================= RABBITMQ FACTORY (AZURE SAFE) =================
        var factory = new ConnectionFactory
        {
            HostName = _rabbit.HostName,
            Port = _rabbit.Port,
            UserName = _rabbit.UserName,
            Password = _rabbit.Password,

            DispatchConsumersAsync = true,

            // 🔥 AZURE RELIABILITY FIXES
            AutomaticRecoveryEnabled = true,
            TopologyRecoveryEnabled = true,
            NetworkRecoveryInterval = TimeSpan.FromSeconds(10)
        };

        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();

        // ================= PREFETCH (IMPORTANT) =================
        _channel.BasicQos(0, 10, false);

        // ================= EXCHANGE =================
        _channel.ExchangeDeclare(
            exchange: _rabbit.DeadLetterExchange,
            type: ExchangeType.Direct,
            durable: true,
            autoDelete: false
        );

        // ================= QUEUES =================
        _channel.QueueDeclare(dlqName, durable: true, exclusive: false, autoDelete: false);

        _channel.QueueDeclare(
            queue: queueName,
            durable: true,
            exclusive: false,
            autoDelete: false,
            arguments: new Dictionary<string, object>
            {
                { "x-dead-letter-exchange", _rabbit.DeadLetterExchange },
                { "x-dead-letter-routing-key", _rabbit.DeadLetterRoutingKey }
            });

        // ================= CONSUMER =================
        var consumer = new AsyncEventingBasicConsumer(_channel);

        consumer.Received += async (sender, args) =>
        {
            if (stoppingToken.IsCancellationRequested)
                return;

            try
            {
                await HandleMessage(args, stoppingToken);
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Unhandled consumer exception");
            }
        };

        _channel.BasicConsume(queueName, autoAck: false, consumer);

        // ================= RETRY LOOP =================
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await _retryProcessor.ProcessAsync(stoppingToken);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error while processing failed emails");
            }

            await Task.Delay(TimeSpan.FromMinutes(5), stoppingToken);
        }
    }

    // ==========================================================
    // MESSAGE HANDLER (ISOLATED FOR SAFETY)
    // ==========================================================
    private async Task HandleMessage(BasicDeliverEventArgs args, CancellationToken token)
    {
        var messageJson = Encoding.UTF8.GetString(args.Body.ToArray());

        UserRegisteredEvent? userEvent = null;

        try
        {
            userEvent = JsonSerializer.Deserialize<UserRegisteredEvent>(messageJson);

            if (userEvent is null)
            {
                Log.Warning("Invalid message received");
                _channel!.BasicNack(args.DeliveryTag, false, false);
                return;
            }

            using (LogContext.PushProperty("Email", userEvent.Email))
            {
                Log.Information("Processing email event");

                await _emailRetryPolicy.ExecuteAsync(async () =>
                {
                    await _emailService.SendWelcomeEmail(
                        userEvent.Email,
                        userEvent.FullName);
                });

                Log.Information("Email sent successfully");

                _channel!.BasicAck(args.DeliveryTag, false);
            }
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Failed processing message for {Email}", userEvent?.Email);

            using var scope = _scopeFactory.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<IEmailDbContext>();

            var failedMessage = new FailedEmailMessage
            {
                Id = Guid.NewGuid(),
                Email = userEvent?.Email ?? "unknown",
                Payload = messageJson,
                ErrorMessage = ex.Message,
                FailedAt = DateTime.UtcNow,
                IsReprocessed = false,
                RetryCount = 0
            };

            await dbContext.FailedEmailMessages.AddAsync(failedMessage, token);
            await dbContext.SaveChangesAsync(token);

            _channel!.BasicNack(args.DeliveryTag, false, false);
        }
    }

    public override void Dispose()
    {
        try
        {
            _channel?.Close();
            _channel?.Dispose();

            _connection?.Close();
            _connection?.Dispose();
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error during RabbitMQ cleanup");
        }

        base.Dispose();
    }
}