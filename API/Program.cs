using Application.Common.Interfaces;
using Application.Features.Tenants;
using AuthService.Application.Features.Users.Commands.CreateUser;
using Domain.Entities ;
using FluentValidation;
using Infrastructure.Identity;
using Infrastructure.Persistence;
using Infrastructure.Persistence.Repositories;
using Infrastructure.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

#region ----------------------------------------------------
// DATABASE
// ----------------------------------------------------------
// Registers EF Core DbContext with SQL Server.
// This is the central persistence layer for AuthService.
#endregion

builder.Services.AddDbContext<AuthDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("AuthDbConnection")));

#region ----------------------------------------------------
// IDENTITY CONFIGURATION (SaaS + GUID BASED)
// ----------------------------------------------------------
// IMPORTANT:
// We use IdentityUser<Guid> + IdentityRole<Guid>
// to ensure consistency with SaaS multi-tenant design.
#endregion

builder.Services
    .AddIdentity<AppUser, IdentityRole<Guid>>(options =>
    {
        options.Password.RequiredLength = 8;
        options.Password.RequireDigit = true;
        options.Password.RequireUppercase = true;
        options.Password.RequireLowercase = true;
        options.Password.RequireNonAlphanumeric = false;

        options.User.RequireUniqueEmail = true;
        options.Lockout.MaxFailedAccessAttempts = 5;
        options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(10);
    })
    .AddEntityFrameworkStores<AuthDbContext>()
    .AddDefaultTokenProviders();

#region ----------------------------------------------------
// MEDIATR (CQRS)
// ----------------------------------------------------------
// Registers Application layer commands/queries.
#endregion

builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(typeof(CreateTenantCommand).Assembly));

builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(typeof(CreateUserCommand).Assembly));

builder.Services.AddValidatorsFromAssemblyContaining<CreateUserCommandValidator>();

#region ----------------------------------------------------
// REPOSITORY + UNIT OF WORK
// ----------------------------------------------------------
// Generic repository abstraction for Clean Architecture.
#endregion

builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<ITenantRepository, TenantRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

#region ----------------------------------------------------
// INFRASTRUCTURE SERVICES
// ----------------------------------------------------------
// Tenant + Identity + HTTP Context services
#endregion

builder.Services.AddHttpContextAccessor();

builder.Services.AddScoped<ICurrentTenantService, CurrentTenantService>();
builder.Services.AddScoped<IIdentityService, IdentityService>();

#region ----------------------------------------------------
// CONTROLLERS + SWAGGER
// ----------------------------------------------------------
// API configuration for AuthService endpoints
#endregion

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

#region ----------------------------------------------------
// HTTP PIPELINE
// ----------------------------------------------------------
// Middleware pipeline configuration
#endregion

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

#region ----------------------------------------------------
// DATABASE SEEDER
// ----------------------------------------------------------
// Seeds:
// - Roles
// - Default tenant (if required)
// - Admin users
#endregion

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    var context = services.GetRequiredService<AuthDbContext>();
    var userManager = services.GetRequiredService<UserManager<Domain.Entities.AppUser>>();
    var roleManager = services.GetRequiredService<RoleManager<IdentityRole<Guid>>>();

    await AuthDbSeeder.SeedAsync(context, userManager, roleManager);
}

app.Run();