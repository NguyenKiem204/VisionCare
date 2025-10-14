using VisionCare.Application.DTOs.Auth;

namespace VisionCare.Application.Interfaces.Auth;

public interface IAuthService
{
    Task<TokenResponse> LoginAsync(LoginRequest request, string ipAddress);
    Task<TokenResponse> RefreshAsync(string refreshToken, string ipAddress);
    Task RevokeAsync(string refreshToken, string ipAddress);
}
