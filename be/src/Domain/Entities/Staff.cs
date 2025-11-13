using VisionCare.Domain.Common;

namespace VisionCare.Domain.Entities;

public class Staff : BaseEntity
{
    public int? AccountId { get; set; }
    public string? StaffName { get; set; }
    public string? Gender { get; set; }
    public DateOnly? Dob { get; set; }
    public string? Address { get; set; }
    public string? Phone { get; set; }
    public string? Avatar { get; set; }

    // Navigation properties
    public User? Account { get; set; }
}
