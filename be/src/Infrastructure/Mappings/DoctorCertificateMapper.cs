using VisionCare.Domain.Entities;

namespace VisionCare.Infrastructure.Mappings;

public static class DoctorCertificateMapper
{
    public static DoctorCertificate ToDomain(VisionCare.Infrastructure.Models.Certificatedoctor model)
    {
        return new DoctorCertificate
        {
            Id = 0, // Composite key, will be handled differently
            DoctorId = model.DoctorId,
            CertificateId = model.CertificateId,
            IssuedDate = model.IssuedDate,
            IssuedBy = model.IssuedBy,
            CertificateImage = model.CertificateImage,
            ExpiryDate = model.ExpiryDate,
            Status = model.Status,
            Created = DateTime.UtcNow,
            LastModified = DateTime.UtcNow
        };
    }

    public static VisionCare.Infrastructure.Models.Certificatedoctor ToInfrastructure(DoctorCertificate domain)
    {
        return new VisionCare.Infrastructure.Models.Certificatedoctor
        {
            DoctorId = domain.DoctorId,
            CertificateId = domain.CertificateId,
            IssuedDate = domain.IssuedDate,
            IssuedBy = domain.IssuedBy,
            CertificateImage = domain.CertificateImage,
            ExpiryDate = domain.ExpiryDate,
            Status = domain.Status
        };
    }
}
