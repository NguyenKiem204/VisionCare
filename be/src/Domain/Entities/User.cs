using VisionCare.Domain.Common;

namespace VisionCare.Domain.Entities;

public class User : BaseEntity
{
    public string Username { get; set; } = string.Empty;
    public string? Password { get; set; }
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }
    public DateTime? CreatedDate { get; set; }
    public int? RoleId { get; set; }
    public string? GoogleId { get; set; }
    public string? FacebookId { get; set; }
    public string? FirstConfirm { get; set; }
    public string? StatusAccount { get; set; }

    // Navigation properties
    public Role? Role { get; set; }
    public Doctor? Doctor { get; set; }
    public Customer? Customer { get; set; }
    public Staff? Staff { get; set; }
}
