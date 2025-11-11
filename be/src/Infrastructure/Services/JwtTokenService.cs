using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using VisionCare.Application.Interfaces.Auth;

namespace VisionCare.Infrastructure.Services;

public class JwtTokenService : IJwtTokenService
{
    private readonly IConfiguration _config;

    public JwtTokenService(IConfiguration config)
    {
        _config = config;
    }

    public (string token, DateTime expiresAt) GenerateAccessToken(
        int accountId,
        string email,
        string displayName,
        string roleName
    )
    {
        var jwtSection = _config.GetSection("Jwt");
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSection["Secret"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, accountId.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, email),
            new Claim(ClaimTypes.Name, displayName),
            new Claim(ClaimTypes.Role, roleName),
        };

        var expiresMinutes = int.TryParse(jwtSection["AccessTokenMinutes"], out var m) ? m : 5;
        var token = new JwtSecurityToken(
            issuer: jwtSection["Issuer"],
            audience: jwtSection["Audience"],
            claims: claims,
            expires: DateTime.Now.AddMinutes(expiresMinutes),
            signingCredentials: creds
        );
        return (new JwtSecurityTokenHandler().WriteToken(token), token.ValidTo);
    }

    public (string refreshToken, DateTime expiresAt, string refreshTokenHash) GenerateRefreshToken()
    {
        var jwtSection = _config.GetSection("Jwt");
        var refreshTokenRaw =
            Convert.ToBase64String(Guid.NewGuid().ToByteArray()) + Guid.NewGuid().ToString("N");
        var refreshExpiresDays = int.TryParse(jwtSection["RefreshTokenDays"], out var d) ? d : 7;
        var expiresAt = DateTime.Now.AddDays(refreshExpiresDays);
        return (refreshTokenRaw, expiresAt, Hash(refreshTokenRaw));
    }

    public string Hash(string value)
    {
        return BCrypt.Net.BCrypt.HashPassword(value, 10);
    }
}
