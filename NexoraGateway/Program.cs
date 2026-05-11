using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);

#region Configuration

builder.Configuration
    .SetBasePath(builder.Environment.ContentRootPath)
    .AddJsonFile("ocelot.json", optional: false, reloadOnChange: true)
    .AddEnvironmentVariables();

#endregion

#region Services

builder.Services.AddOcelot();

#endregion

var app = builder.Build();

#region Middleware

await app.UseOcelot();

#endregion

app.Run();