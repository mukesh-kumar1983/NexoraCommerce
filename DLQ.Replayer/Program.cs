using RabbitMQ.Client;
using Serilog;
using System.Text;

const string DlqQueue = "user.registered.dlq";
const string MainQueue = "user.registered.queue";

// ------------------------------------------------------------
// Configure Serilog
// ------------------------------------------------------------
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateLogger();

try
{
    Log.Information("Starting DLQ Replayer...");

    // ------------------------------------------------------------
    // RabbitMQ Connection
    // ------------------------------------------------------------
    var factory = new ConnectionFactory()
    {
        HostName = "localhost",
        UserName = "admin",
        Password = "admin123"
    };

    using var connection = factory.CreateConnection();
    using var channel = connection.CreateModel();

    Log.Information("Connected to RabbitMQ");

    // ------------------------------------------------------------
    // Read Messages from DLQ
    // ------------------------------------------------------------
    Log.Information("Reading messages from DLQ: {Queue}", DlqQueue);

    var result = channel.BasicGet(DlqQueue, autoAck: false);

    if (result == null)
    {
        Log.Information("No messages found in DLQ");
        return;
    }

    while (result != null)
    {
        var body = result.Body.ToArray();
        var message = Encoding.UTF8.GetString(body);

        Log.Warning("Failed Message Found:");
        Log.Warning("{Message}", message);

        Console.WriteLine();
        Console.WriteLine("Replay this message? (y/n)");

        var input = Console.ReadLine();

        if (input?.ToLower() == "y")
        {
            // ----------------------------------------------------
            // Replay message back to main queue
            // ----------------------------------------------------
            channel.BasicPublish(
                exchange: "",
                routingKey: MainQueue,
                basicProperties: null,
                body: body
            );

            Log.Information(
                "Message replayed from DLQ [{Dlq}] to Main Queue [{Main}]",
                DlqQueue,
                MainQueue
            );

            // Remove message from DLQ
            channel.BasicAck(result.DeliveryTag, false);

            Log.Information("DLQ message acknowledged and removed");
        }
        else
        {
            Log.Warning("Replay skipped by user");

            // Reject permanently
            channel.BasicNack(
                result.DeliveryTag,
                false,
                false
            );
        }

        // Read next message
        result = channel.BasicGet(DlqQueue, autoAck: false);
    }

    Log.Information("DLQ processing completed");
}
catch (Exception ex)
{
    Log.Fatal(ex, "DLQ Replayer crashed unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}