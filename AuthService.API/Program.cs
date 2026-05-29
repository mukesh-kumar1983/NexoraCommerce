using AuthService.API.Extensions;
using AuthService.API.Extentions;
using AuthService.Infrastructure.Persistence;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

#region Serilog
builder.Host.UseSerilogLogging();
#endregion

#region Services Registration
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddAuthenticationServices(builder.Configuration);
builder.Services.AddCorsPolicy();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();


#endregion

var app = builder.Build();

if (builder.Environment.IsDevelopment())
{
    builder.Configuration.AddJsonFile("appsettings.Development.json", optional: true);
}
else if (builder.Environment.IsStaging())
{
    builder.Configuration.AddJsonFile("appsettings.Staging.json", optional: true);
}
else
{
    builder.Configuration.AddJsonFile("appsettings.Production.json", optional: true);
}


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

#region Middleware Pipeline
app.UseApplicationPipeline(app.Environment);
app.UseCors("DefaultCors");
app.UseAuthentication();
app.UseAuthorization();
#endregion

#region Database Migration & Seeding
await app.MigrateAndSeedDatabaseAsync();
#endregion

app.Run();