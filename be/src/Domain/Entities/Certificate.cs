using VisionCare.Domain.Common;

namespace VisionCare.Domain.Entities;

public class Certificate : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    
    // Navigation properties
    public ICollection<DoctorCertificate> DoctorCertificates { get; set; } = new List<DoctorCertificate>();
}
