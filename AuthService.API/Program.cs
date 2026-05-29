using AuthService.API.Extensions;
using AuthService.API.Extentions;
using AuthService.Application.Common.Interfaces;
using AuthService.Application.Common.Settings;
using AuthService.Infrastructure.Persistence;
using AuthService.Infrastructure.Repositories;
using AuthService.Infrastructure.Services;
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


#endregion

var app = builder.Build();

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