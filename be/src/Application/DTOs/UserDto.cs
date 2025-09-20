namespace VisionCare.Application.DTOs;

public class UserDto
{
    public int Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }
    public string? RoleName { get; set; }
    public string? StatusAccount { get; set; }
}
