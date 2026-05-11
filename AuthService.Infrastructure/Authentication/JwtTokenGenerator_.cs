using AuthService.Application.Common.Interfaces;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;
using AuthService.Domain.Entities;


namespace AuthService.Infrastructure.Authentication;

public class JwtTokenGenerator_ : IJwtTokenGenerator_
{
    private readonly IConfiguration _configuration;

    public JwtTokenGenerator_(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string GenerateToken(AppUser user, IEnumerable<string> roles)
    {
        var claims = new List<Claim>
    {
        new Claim(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Sub, user.Id.ToString()),
        new Claim(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Email, user.Email!),
        new Claim(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()), // Good practice for unique tokens
    };

        // Add each role as a separate claim
        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        var jwtKey = _configuration["Jwt:Key"]
            ?? throw new InvalidOperationException("JWT Key is not configured.");

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));

        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var expires = DateTime.UtcNow.AddMinutes(
            Convert.ToDouble(_configuration["Jwt:DurationInMinutes"] ?? "60"));

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: expires,
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public string GenerateRefreshToken()
    {
        var randomNumber = new byte[64];

        using var rng = RandomNumberGenerator.Create();

        rng.GetBytes(randomNumber);

        return Convert.ToBase64String(randomNumber);
    }
}