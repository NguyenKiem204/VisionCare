namespace VisionCare.Application.DTOs.DoctorCertificateDto;

public class DoctorCertificateDto
{
    public int Id { get; set; }
    public int DoctorId { get; set; }
    public int CertificateId { get; set; }
    public string? DoctorName { get; set; }
    public string? CertificateName { get; set; }
    public DateOnly? IssuedDate { get; set; }
    public string? IssuedBy { get; set; }
    public string? CertificateImage { get; set; }
    public DateOnly? ExpiryDate { get; set; }
    public string? Status { get; set; }
    public DateTime Created { get; set; }
    public DateTime? LastModified { get; set; }
}

public class CreateDoctorCertificateRequest
{
    public int DoctorId { get; set; }
    public int CertificateId { get; set; }
    public DateOnly? IssuedDate { get; set; }
    public string? IssuedBy { get; set; }
    public string? CertificateImage { get; set; }
    public DateOnly? ExpiryDate { get; set; }
    public string? Status { get; set; } = "Active";
}

public class UpdateDoctorCertificateRequest
{
    public DateOnly? IssuedDate { get; set; }
    public string? IssuedBy { get; set; }
    public string? CertificateImage { get; set; }
    public DateOnly? ExpiryDate { get; set; }
    public string? Status { get; set; }
}
