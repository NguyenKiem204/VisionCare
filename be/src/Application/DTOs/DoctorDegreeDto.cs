namespace VisionCare.Application.DTOs.DoctorDegreeDto;

public class DoctorDegreeDto
{
    public int Id { get; set; }
    public int DoctorId { get; set; }
    public int DegreeId { get; set; }
    public string? DoctorName { get; set; }
    public string? DegreeName { get; set; }
    public DateOnly? IssuedDate { get; set; }
    public string? IssuedBy { get; set; }
    public string? CertificateImage { get; set; }
    public string? Status { get; set; }
    public DateTime Created { get; set; }
    public DateTime? LastModified { get; set; }
}

public class CreateDoctorDegreeRequest
{
    public int DoctorId { get; set; }
    public int DegreeId { get; set; }
    public DateOnly? IssuedDate { get; set; }
    public string? IssuedBy { get; set; }
    public string? CertificateImage { get; set; }
    public string? Status { get; set; } = "Active";
}

public class UpdateDoctorDegreeRequest
{
    public DateOnly? IssuedDate { get; set; }
    public string? IssuedBy { get; set; }
    public string? CertificateImage { get; set; }
    public string? Status { get; set; }
}
