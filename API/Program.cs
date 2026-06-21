using Application.Common.Interfaces;
using Application.Features.Tenants;
using AuthService.Application.Features.Users.Commands.CreateUser;
using Domain.Entities;
using FluentValidation;
using Infrastructure.Identity;
using Infrastructure.Persistence;
using Infrastructure.Persistence.Repositories;
using Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NexoraEnterprise.SharedInfrastructure.Authorization;
using NexoraEnterprise.SharedInfrastructure.Middleware;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

#region DATABASE

builder.Services.AddDbContext<AuthDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("AuthDbConnection")));

builder.Services.AddScoped<IAuthDbContext>(provider =>
    provider.GetRequiredService<AuthDbContext>());

#endregion

#region IDENTITY

builder.Services
    .AddIdentity<AppUser, Role>(options =>
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

#endregion

#region JWT

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,

            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!)
            )
        };
    });

#endregion

#region CORS

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular", policy =>
    {
        policy
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials()
            .WithOrigins("http://localhost:4200");
    });
});

#endregion

#region APPLICATION SERVICES

builder.Services.AddScoped<IJwtTokenService, JwtTokenService>();
builder.Services.AddScoped<ITenantResolverService, TenantResolverService>();
builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();
builder.Services.AddScoped<ICurrentTenantService, CurrentTenantService>();
builder.Services.AddScoped<IIdentityService, IdentityService>();

#endregion

#region REPOSITORIES

builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<ITenantRepository, TenantRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

#endregion

#region MEDIATR

builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssemblies(
        typeof(CreateTenantCommand).Assembly,
        typeof(CreateUserCommand).Assembly
    ));

#endregion

#region VALIDATION

builder.Services.AddValidatorsFromAssemblyContaining<CreateTenantCommand>();

#endregion

#region AUTHORIZATION

builder.Services.AddAuthorization(options =>
{
    PolicyProvider.AddPermissionPolicies(options);
});

#endregion

#region CONTROLLERS

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

#endregion

var app = builder.Build();

#region PIPELINE

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// ❗ MUST be first in pipeline
app.UseMiddleware<GlobalExceptionMiddleware>();

app.UseRouting();

app.UseCors("AllowAngular");

app.UseAuthentication();

app.UseMiddleware<TenantContextMiddleware>();
app.UseMiddleware<TenantIsolationMiddleware>();


app.UseAuthorization();

app.MapControllers();

#endregion

app.Run();


//using Application.Common.Interfaces;
//using Application.Features.Tenants;
//using AuthService.Application.Features.Users.Commands.CreateUser;
//using Domain.Entities;
//using FluentValidation;
//using Infrastructure.Identity;
//using Infrastructure.Persistence;
//using Infrastructure.Persistence.Repositories;
//using Infrastructure.Services;
//using Microsoft.AspNetCore.Authentication.JwtBearer;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.IdentityModel.Tokens;
//using NexoraEnterprise.SharedInfrastructure.Authorization;
//using NexoraEnterprise.SharedInfrastructure.Middleware;
//using System.Text;

//var builder = WebApplication.CreateBuilder(args);

//// ---------------- DATABASE ----------------
//builder.Services.AddDbContext<AuthDbContext>(options =>
//    options.UseSqlServer(builder.Configuration.GetConnectionString("AuthDbConnection")));

//// ---------------- IDENTITY ----------------
//builder.Services.AddIdentity<AppUser, IdentityRole>(options =>
//{
//    options.Password.RequiredLength = 8;
//    options.Password.RequireDigit = true;
//    options.Password.RequireUppercase = true;
//    options.Password.RequireLowercase = true;
//    options.Password.RequireNonAlphanumeric = false;

//    options.User.RequireUniqueEmail = true;
//    options.Lockout.MaxFailedAccessAttempts = 5;
//    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(10);
//})
//.AddEntityFrameworkStores<AuthDbContext>()
//.AddDefaultTokenProviders();

//// ---------------- JWT ----------------
//builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
//.AddJwtBearer(options =>
//{
//    options.TokenValidationParameters = new TokenValidationParameters
//    {
//        ValidateIssuer = true,
//        ValidateAudience = true,
//        ValidateLifetime = true,
//        ValidateIssuerSigningKey = true,

//        ValidIssuer = builder.Configuration["Jwt:Issuer"],
//        ValidAudience = builder.Configuration["Jwt:Audience"],
//        IssuerSigningKey = new SymmetricSecurityKey(
//            Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!)
//        )
//    };
//});

//// ---------------- CORS ----------------
//builder.Services.AddCors(options =>
//{
//    options.AddPolicy("AllowAngular", policy =>
//    {
//        policy
//            .AllowAnyHeader()
//            .AllowAnyMethod()
//            .AllowCredentials()
//            .WithOrigins("http://localhost:4200");
//    });
//});

//builder.Services.AddScoped<IJwtTokenService, JwtTokenService>();
//builder.Services.AddScoped<ITenantResolverService, TenantResolverService>();
//builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();

//builder.Services.AddAuthorization(options =>
//{
//    PolicyProvider.AddPermissionPolicies(options);
//});

//// ---------------- MEDIATR ----------------
//builder.Services.AddMediatR(cfg =>
//    cfg.RegisterServicesFromAssemblies(
//        typeof(CreateTenantCommand).Assembly,
//        typeof(CreateUserCommand).Assembly
//    ));

//// ---------------- VALIDATORS ----------------
//builder.Services.AddValidatorsFromAssemblyContaining<CreateUserCommand>();

//// ---------------- REPOSITORIES ----------------
//builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
//builder.Services.AddScoped<ITenantRepository, TenantRepository>();
//builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

//// ---------------- SERVICES ----------------
//builder.Services.AddHttpContextAccessor();
//builder.Services.AddScoped<ICurrentTenantService, CurrentTenantService>();
//builder.Services.AddScoped<IIdentityService, IdentityService>();

//// ---------------- CONTROLLERS ----------------
//builder.Services.AddControllers();
//builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();

//var app = builder.Build();

//// ---------------- PIPELINE ----------------
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}

//app.UseHttpsRedirection();

//app.UseMiddleware<GlobalExceptionMiddleware>();

//app.UseRouting();

//app.UseCors("AllowAngular");

//app.UseAuthentication();

//app.UseMiddleware<TenantContextMiddleware>();

//app.UseMiddleware<TenantIsolationMiddleware>();

//app.UseAuthorization();

//app.MapControllers();

//app.Run();