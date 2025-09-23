using VisionCare.Domain.Common;

namespace VisionCare.Domain.Entities;

public class Role : BaseEntity
{
    public string RoleName { get; set; } = string.Empty;

    // Navigation properties
    public ICollection<User> Users { get; set; } = new List<User>();
}
