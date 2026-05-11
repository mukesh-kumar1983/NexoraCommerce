using Serilog;

namespace AuthService.API.Extensions;

public static class SerilogExtensions
{
    public static IHostBuilder UseSerilogLogging(this IHostBuilder host)
    {
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Information()
            .MinimumLevel.Override("Microsoft", Serilog.Events.LogEventLevel.Warning)
            .Enrich.FromLogContext()
            .Enrich.WithMachineName()
            .Enrich.WithThreadId()
            .WriteTo.Console()
            .WriteTo.File("Logs/log-.txt", rollingInterval: RollingInterval.Day)
            .CreateLogger();

        return host.UseSerilog();
    }
}