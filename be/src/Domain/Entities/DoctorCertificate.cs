using VisionCare.Domain.Common;

namespace VisionCare.Domain.Entities;

public class DoctorCertificate : BaseEntity
{
    public int DoctorId { get; set; }
    public int CertificateId { get; set; }
    public DateOnly? IssuedDate { get; set; }
    public string? IssuedBy { get; set; }
    public string? CertificateImage { get; set; }
    public DateOnly? ExpiryDate { get; set; }
    public string? Status { get; set; } = "Active";
    
    // Navigation properties
    public Doctor? Doctor { get; set; }
    public Certificate? Certificate { get; set; }
}
