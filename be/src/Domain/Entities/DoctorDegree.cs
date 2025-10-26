using VisionCare.Domain.Common;

namespace VisionCare.Domain.Entities;

public class DoctorDegree : BaseEntity
{
    public int DoctorId { get; set; }
    public int DegreeId { get; set; }
    public DateOnly? IssuedDate { get; set; }
    public string? IssuedBy { get; set; }
    public string? CertificateImage { get; set; }
    public string? Status { get; set; } = "Active";
    
    // Navigation properties
    public Doctor? Doctor { get; set; }
    public Degree? Degree { get; set; }
}
