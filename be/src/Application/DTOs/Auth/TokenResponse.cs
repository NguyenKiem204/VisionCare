namespace VisionCare.Application.DTOs.Auth;

public class TokenResponse
{
    public string AccessToken { get; set; } = string.Empty;
    public DateTime ExpiresAt { get; set; }
    public string RefreshToken { get; set; } = string.Empty;
}
