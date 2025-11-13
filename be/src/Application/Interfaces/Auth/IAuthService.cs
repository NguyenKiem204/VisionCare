using VisionCare.Application.DTOs.Auth;
using VisionCare.Application.DTOs.User;

namespace VisionCare.Application.Interfaces.Auth;

public interface IAuthService
{
    Task<TokenResponse> LoginAsync(LoginRequest request, string ipAddress);
    Task<TokenResponse> RefreshTokenAsync(string refreshToken, string ipAddress);
    Task RevokeTokenAsync(string refreshToken, string ipAddress);
    Task<UserDto> RegisterAsync(RegisterRequest request);
    Task<bool> ChangePasswordAsync(int userId, string currentPassword, string newPassword);
    Task<bool> ResetPasswordAsync(string email);
    Task<bool> ConfirmPasswordResetAsync(string token, string newPassword);
    Task<bool> ConfirmEmailAsync(string token);
    Task<bool> ResendEmailConfirmationAsync(string email);
    Task<bool> ValidateTokenAsync(string token);
    Task<UserDto?> GetUserFromTokenAsync(string token);
}
