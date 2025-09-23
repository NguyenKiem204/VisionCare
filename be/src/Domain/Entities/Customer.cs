using VisionCare.Domain.Common;

namespace VisionCare.Domain.Entities;

public class Customer : BaseEntity
{
    public int? AccountId { get; set; }
    public string? CustomerName { get; set; }
    public string? Gender { get; set; }
    public DateOnly? Dob { get; set; }
    public string? Address { get; set; }
    public User? Account { get; set; }
}
