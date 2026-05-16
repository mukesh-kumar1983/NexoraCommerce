using EmailService.Application.Interfaces;
using EmailService.Infrastructure.Persistence;
using EmailService.Worker;
using EmailService.Worker.Services;
using Microsoft.EntityFrameworkCore;
using Serilog;
using EmailService.Application.Common.Settings;
using Microsoft.Extensions.Options;

var builder = Host.CreateApplicationBuilder(args);

#region ---------------- SERILOG ----------------

// Create logger FIRST
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateLogger();

// IMPORTANT: replace default logging with Serilog
builder.Services.AddLogging(logging =>
{
    logging.ClearProviders();
    logging.AddSerilog();
});

#endregion

#region ---------------- CONFIGURATION SETTINGS ----------------

builder.Services.Configure<RabbitMqSettings>(
    builder.Configuration.GetSection("RabbitMQ"));

builder.Services.Configure<EmailSettings>(
    builder.Configuration.GetSection("EmailSettings"));

#endregion

#region ---------------- DATABASE ----------------

builder.Services.AddDbContext<EmailDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("EmailDb")));

builder.Services.AddScoped<IEmailDbContext, EmailDbContext>();

#endregion

#region ---------------- SERVICES ----------------

builder.Services.AddSingleton<IEmailService, EmailService.Worker.Services.EmailService>();

#endregion

#region ---------------- WORKER ----------------

builder.Services.AddHostedService<Worker>();

#endregion

var host = builder.Build();
host.Run();