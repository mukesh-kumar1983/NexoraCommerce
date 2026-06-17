using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Application.Common.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.Identity;

public class JwtTokenService : IJwtTokenService
{
    private readonly IConfiguration _config;
    private readonly UserManager<AppUser> _userManager;

    public JwtTokenService(
        IConfiguration config,
        UserManager<AppUser> userManager)
    {
        _config = config;
        _userManager = userManager;
    }

    public async Task<(string Token, DateTime ExpiresAt, List<string> Roles)> GenerateTokenAsync(AppUser user)
    {
        // ====================================================
        // 1. LOAD ROLES SAFELY (PRODUCTION SAFE WAY)
        // ====================================================
        var roles = (await _userManager.GetRolesAsync(user)).ToList();

        // ====================================================
        // 2. BUILD CLAIMS
        // ====================================================

        var isSuperAdmin = await _userManager.IsInRoleAsync(user, "SuperAdmin");

        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email ?? ""),
            new Claim("tenantId", user.TenantId.ToString()),
            new Claim("isSuperAdmin", isSuperAdmin.ToString())
        };

        claims.AddRange(roles.Select(r => new Claim(ClaimTypes.Role, r)));

        // ====================================================
        // 3. TOKEN SIGNING
        // ====================================================
        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));

        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var expiresAt = DateTime.UtcNow.AddHours(2);

        var token = new JwtSecurityToken(
            issuer: _config["Jwt:Issuer"],
            audience: _config["Jwt:Audience"],
            claims: claims,
            expires: expiresAt,
            signingCredentials: creds
        );

        return (new JwtSecurityTokenHandler().WriteToken(token), expiresAt, roles);
    }
}