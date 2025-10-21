using VisionCare.Application.DTOs.Auth;
using VisionCare.Application.DTOs.User;

namespace VisionCare.Application.Interfaces.Auth;

/// <summary>
/// Core authentication service for login, logout, and token management
/// </summary>
public interface IAuthService
{
    // Authentication
    Task<TokenResponse> LoginAsync(LoginRequest request, string ipAddress);
    Task<TokenResponse> RefreshTokenAsync(string refreshToken, string ipAddress);
    Task RevokeTokenAsync(string refreshToken, string ipAddress);

    // Registration
    Task<UserDto> RegisterAsync(RegisterRequest request);

    // Password Management
    Task<bool> ChangePasswordAsync(int userId, string currentPassword, string newPassword);
    Task<bool> ResetPasswordAsync(string email);
    Task<bool> ConfirmPasswordResetAsync(string token, string newPassword);

    // Email Verification
    Task<bool> ConfirmEmailAsync(string token);
    Task<bool> ResendEmailConfirmationAsync(string email);

    // Token Validation
    Task<bool> ValidateTokenAsync(string token);
    Task<UserDto?> GetUserFromTokenAsync(string token);
}
