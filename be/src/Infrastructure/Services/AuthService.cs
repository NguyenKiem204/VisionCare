using System.Net;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using VisionCare.Application.DTOs.Auth;
using VisionCare.Application.Interfaces.Auth;
using VisionCare.Infrastructure.Data;
using VisionCare.Infrastructure.Models;
using Bcrypt = BCrypt.Net.BCrypt;

namespace VisionCare.Infrastructure.Services;

public class AuthService : IAuthService
{
    private readonly VisionCareDbContext _db;
    private readonly IConfiguration _config;
    private readonly IJwtTokenService _jwt;

    public AuthService(VisionCareDbContext db, IConfiguration config, IJwtTokenService jwt)
    {
        _db = db;
        _config = config;
        _jwt = jwt;
    }

    public async Task<TokenResponse> LoginAsync(LoginRequest request, string ipAddress)
    {
        var account = await _db
            .Accounts.Include(a => a.Role)
            .FirstOrDefaultAsync(a => a.Email == request.Email);
        if (
            account == null
            || string.IsNullOrEmpty(account.PasswordHash)
            || !Bcrypt.Verify(request.Password, account.PasswordHash)
        )
        {
            throw new UnauthorizedAccessException("Invalid credentials");
        }

        return await IssueTokensAsync(account, ipAddress);
    }

    public async Task<TokenResponse> RefreshAsync(string refreshToken, string ipAddress)
    {
        var token = await _db.Refreshtokens.FirstOrDefaultAsync(t =>
            t.TokenHash == Hash(refreshToken) && t.RevokedAt == null
        );
        if (token == null || token.ExpiresAt <= DateTime.Now)
        {
            throw new UnauthorizedAccessException("Invalid refresh token");
        }

        var account = await _db
            .Accounts.Include(a => a.Role)
            .FirstAsync(a => a.AccountId == token.AccountId);

        // revoke old
        token.RevokedAt = DateTime.Now;
        await _db.SaveChangesAsync();

        return await IssueTokensAsync(account, ipAddress);
    }

    public async Task RevokeAsync(string refreshToken, string ipAddress)
    {
        var token = await _db.Refreshtokens.FirstOrDefaultAsync(t =>
            t.TokenHash == Hash(refreshToken) && t.RevokedAt == null
        );
        if (token != null)
        {
            token.RevokedAt = DateTime.Now;
            await _db.SaveChangesAsync();
        }
    }

    private async Task<TokenResponse> IssueTokensAsync(Account account, string ipAddress)
    {
        var (accessToken, accessExpires) = _jwt.GenerateAccessToken(
            account.AccountId,
            account.Email,
            account.Username ?? account.Email,
            account.Role.RoleName
        );

        var (refreshTokenRaw, refreshExpires, refreshHash) = _jwt.GenerateRefreshToken();
        var refreshEntity = new Refreshtoken
        {
            AccountId = account.AccountId,
            TokenHash = refreshHash,
            CreatedAt = DateTime.Now,
            ExpiresAt = refreshExpires,
            CreatedByIp = IPAddress.TryParse(ipAddress, out var ip) ? ip : null,
        };
        _db.Refreshtokens.Add(refreshEntity);
        await _db.SaveChangesAsync();

        return new TokenResponse
        {
            AccessToken = accessToken,
            ExpiresAt = accessExpires,
            RefreshToken = refreshTokenRaw,
        };
    }

    private static string Hash(string value) => Bcrypt.HashPassword(value);
}
