using AuthService.API.Extensions;
using AuthService.API.Extentions;
using AuthService.Application.Common.Interfaces;
using AuthService.Application.Common.Settings;
using AuthService.Infrastructure.Persistence;
using AuthService.Infrastructure.Repositories;
using AuthService.Infrastructure.Services;
using Serilog;
using QuestPDF.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

#region Serilog
builder.Host.UseSerilogLogging();
#endregion

#region Services Registration

builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddAuthenticationServices(builder.Configuration);
builder.Services.AddCorsPolicy();

builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ICurrentTenantService, CurrentTenantService>();
builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();

builder.Services.Configure<AzureBlobSettings>(
    builder.Configuration.GetSection("AzureBlobSettings"));

builder.Services.AddScoped<IAzureBlobService, AzureBlobService>();
builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();

QuestPDF.Settings.License = LicenseType.Community;


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