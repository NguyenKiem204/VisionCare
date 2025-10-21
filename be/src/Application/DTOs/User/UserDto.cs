namespace VisionCare.Application.DTOs.User;

public class UserDto
{
    public int Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public string? Email { get; set; }
    public string? RoleName { get; set; }
    public string? StatusAccount { get; set; }
}


