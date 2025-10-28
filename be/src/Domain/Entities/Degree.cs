using VisionCare.Domain.Common;

namespace VisionCare.Domain.Entities;

public class Degree : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    
    // Navigation properties
    public ICollection<DoctorDegree> DoctorDegrees { get; set; } = new List<DoctorDegree>();
}
