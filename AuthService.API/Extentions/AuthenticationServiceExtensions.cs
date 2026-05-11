using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace AuthService.API.Extensions;

public static class AuthenticationServiceExtensions
{
    public static IServiceCollection AddAuthenticationServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var jwtSection = configuration.GetSection("Jwt");

        var key = jwtSection["Key"];
        var issuer = jwtSection["Issuer"];
        var audience = jwtSection["Audience"];

        if (string.IsNullOrWhiteSpace(key))
            throw new Exception("JWT Key is missing in configuration.");

        if (string.IsNullOrWhiteSpace(issuer))
            throw new Exception("JWT Issuer is missing in configuration.");

        if (string.IsNullOrWhiteSpace(audience))
            throw new Exception("JWT Audience is missing in configuration.");

        var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));

        services
            .AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = true; // enable in production
                options.SaveToken = true;

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,

                    ValidIssuer = issuer,
                    ValidAudience = audience,
                    IssuerSigningKey = signingKey,

                    ClockSkew = TimeSpan.Zero // important: removes 5 min default delay
                };
            });

        services.AddAuthorization();

        return services;
    }
}