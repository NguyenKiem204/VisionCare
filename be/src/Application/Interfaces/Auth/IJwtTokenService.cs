namespace VisionCare.Application.Interfaces.Auth;

public interface IJwtTokenService
{
    (string token, DateTime expiresAt) GenerateAccessToken(
        int accountId,
        string email,
        string displayName,
        string roleName
    );

    (string refreshToken, DateTime expiresAt, string refreshTokenHash) GenerateRefreshToken();

    string Hash(string value);
}
