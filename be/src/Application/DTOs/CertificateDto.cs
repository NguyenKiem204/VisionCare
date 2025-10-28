namespace VisionCare.Application.DTOs.CertificateDto;

public class CertificateDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public DateTime Created { get; set; }
    public DateTime? LastModified { get; set; }
}

public class CreateCertificateRequest
{
    public string Name { get; set; } = string.Empty;
}

public class UpdateCertificateRequest
{
    public string Name { get; set; } = string.Empty;
}
