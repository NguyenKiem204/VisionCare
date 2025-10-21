using System.Net;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using VisionCare.Application.DTOs.Auth;
using VisionCare.Application.DTOs.User;
using VisionCare.Application.Interfaces.Auth;
using VisionCare.Infrastructure.Data;
using VisionCare.Infrastructure.Models;
using Bcrypt = BCrypt.Net.BCrypt;

namespace VisionCare.Infrastructure.Services;

using VisionCare.Application.Interfaces.Auth;

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

    public async Task<TokenResponse> RefreshTokenAsync(string refreshToken, string ipAddress)
    {
        // Get recent valid tokens only (last 100) to limit BCrypt verification
        var validTokens = await _db
            .Refreshtokens.Where(t => t.RevokedAt == null && t.ExpiresAt > DateTime.Now)
            .OrderByDescending(t => t.CreatedAt)
            .Take(100)
            .Select(t => new
            {
                t.TokenId,
                t.TokenHash,
                t.AccountId,
            })
            .ToListAsync();

        // Find the matching token using BCrypt verification
        var tokenMatch = validTokens.FirstOrDefault(t => Bcrypt.Verify(refreshToken, t.TokenHash));
        if (tokenMatch == null)
        {
            throw new UnauthorizedAccessException("Invalid refresh token");
        }

        // Get the full token entity for revocation
        var token = await _db.Refreshtokens.FindAsync(tokenMatch.TokenId);

        var account = await _db
            .Accounts.Include(a => a.Role)
            .FirstAsync(a => a.AccountId == tokenMatch.AccountId);

        // revoke old token
        token.RevokedAt = DateTime.Now;

        // Clean up old expired tokens (keep only last 50 per account)
        var oldTokens = await _db
            .Refreshtokens.Where(t =>
                t.AccountId == tokenMatch.AccountId
                && (t.RevokedAt != null || t.ExpiresAt <= DateTime.Now)
            )
            .OrderByDescending(t => t.CreatedAt)
            .Skip(50)
            .ToListAsync();

        if (oldTokens.Any())
        {
            _db.Refreshtokens.RemoveRange(oldTokens);
        }

        await _db.SaveChangesAsync();

        return await IssueTokensAsync(account, ipAddress);
    }

    public async Task RevokeTokenAsync(string refreshToken, string ipAddress)
    {
        // Get recent non-revoked tokens (limit to reduce BCrypt.Verify calls)
        var validTokens = await _db
            .Refreshtokens.Where(t => t.RevokedAt == null)
            .OrderByDescending(t => t.CreatedAt) // Most recent first
            .Take(50) // Limit to 50 most recent tokens
            .Select(t => new { t.TokenId, t.TokenHash })
            .ToListAsync();

        var tokenMatch = validTokens.FirstOrDefault(t => Bcrypt.Verify(refreshToken, t.TokenHash));
        if (tokenMatch != null)
        {
            var token = await _db.Refreshtokens.FindAsync(tokenMatch.TokenId);
            token.RevokedAt = DateTime.Now;
            await _db.SaveChangesAsync();
        }
    }

    public async Task<UserDto> RegisterAsync(RegisterRequest request)
    {
        // minimal working registration: create Account with hashed password and default role (Customer = 4)
        if (await _db.Accounts.AnyAsync(a => a.Email == request.Email))
        {
            throw new InvalidOperationException("Email already exists");
        }

        var account = new Account
        {
            Email = request.Email,
            Username = request.Username ?? request.Email,
            PasswordHash = string.IsNullOrWhiteSpace(request.Password)
                ? null
                : Bcrypt.HashPassword(request.Password),
            EmailConfirmed = false,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            RoleId = 4,
            Status = "Active",
        };
        _db.Accounts.Add(account);
        await _db.SaveChangesAsync();

        var role = await _db.Roles.FirstOrDefaultAsync(r => r.RoleId == account.RoleId);
        return new UserDto
        {
            Id = account.AccountId,
            Username = account.Username ?? account.Email,
            Email = account.Email,
            RoleName = role?.RoleName,
        };
    }

    public async Task<bool> ChangePasswordAsync(
        int userId,
        string currentPassword,
        string newPassword
    )
    {
        var account = await _db.Accounts.FirstOrDefaultAsync(a => a.AccountId == userId);
        if (
            account == null
            || string.IsNullOrEmpty(account.PasswordHash)
            || !Bcrypt.Verify(currentPassword, account.PasswordHash)
        )
        {
            return false;
        }
        account.PasswordHash = Bcrypt.HashPassword(newPassword);
        account.LastPasswordChange = DateTime.UtcNow;
        account.UpdatedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync();
        return true;
    }

    public async Task<bool> ResetPasswordAsync(string email)
    {
        var account = await _db.Accounts.FirstOrDefaultAsync(a => a.Email == email);
        if (account == null)
        {
            return false;
        }
        account.PasswordResetToken = Guid.NewGuid().ToString("N");
        account.PasswordResetExpires = DateTime.UtcNow.AddHours(1);
        await _db.SaveChangesAsync();
        return true;
    }

    public async Task<bool> ConfirmPasswordResetAsync(string token, string newPassword)
    {
        var account = await _db.Accounts.FirstOrDefaultAsync(a =>
            a.PasswordResetToken == token && a.PasswordResetExpires >= DateTime.UtcNow
        );
        if (account == null)
        {
            return false;
        }
        account.PasswordHash = Bcrypt.HashPassword(newPassword);
        account.PasswordResetToken = null;
        account.PasswordResetExpires = null;
        await _db.SaveChangesAsync();
        return true;
    }

    public async Task<bool> ConfirmEmailAsync(string token)
    {
        var account = await _db.Accounts.FirstOrDefaultAsync(a =>
            a.EmailConfirmationToken == token
        );
        if (account == null)
        {
            return false;
        }
        account.EmailConfirmed = true;
        account.EmailConfirmationToken = null;
        await _db.SaveChangesAsync();
        return true;
    }

    public async Task<bool> ResendEmailConfirmationAsync(string email)
    {
        var account = await _db.Accounts.FirstOrDefaultAsync(a => a.Email == email);
        if (account == null)
        {
            return false;
        }
        account.EmailConfirmationToken = Guid.NewGuid().ToString("N");
        await _db.SaveChangesAsync();
        return true;
    }

    public async Task<bool> ValidateTokenAsync(string token)
    {
        if (string.IsNullOrWhiteSpace(token))
            return false;
        var parts = token.Split('.');
        return await Task.FromResult(parts.Length == 3);
    }

    public async Task<UserDto?> GetUserFromTokenAsync(string token)
    {
        // Not implemented without full claim parsing service; return null for now
        return await Task.FromResult<UserDto?>(null);
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
