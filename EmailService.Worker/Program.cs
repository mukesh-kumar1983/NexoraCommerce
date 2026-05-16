using EmailService.Application.Common.Settings;
using EmailService.Application.Interfaces;
using EmailService.Infrastructure.Persistence;
using EmailService.Worker;
using EmailService.Worker.Processors;
using EmailService.Worker.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Serilog;

var builder = Host.CreateApplicationBuilder(args);

#region ---------------- SERILOG ----------------

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateLogger();

// IMPORTANT: correct way for minimal host
builder.Services.AddSerilog();
#endregion

#region-------------- CONFIGURATION SETTINGS --

builder.Services.Configure<RabbitMqSettings>(
    builder.Configuration.GetSection("RabbitMQ"));

builder.Services.Configure<EmailSettings>(
    builder.Configuration.GetSection("EmailSettings"));

builder.Services.Configure<RetrySettings>(
    builder.Configuration.GetSection("RetrySettings"));

var conn = builder.Configuration.GetConnectionString("DefaultConnection");
Console.WriteLine($"CONNECTION STRING: {conn}");

#endregion

#region ---------------- DATABASE ----------------

builder.Services.AddDbContext<EmailDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IEmailDbContext, EmailDbContext>();

#endregion

#region ---------------- SERVICES ----------------

builder.Services.AddSingleton<IEmailService, EmailService.Worker.Services.EmailService>();
builder.Services.AddScoped<EmailRetryProcessor>();

#endregion

#region ---------------- WORKER ----------------

builder.Services.AddHostedService<Worker>();

#endregion

var host = builder.Build();
host.Run();