using VisionCare.Domain.Entities;

namespace VisionCare.Infrastructure.Mappings;

public static class DoctorDegreeMapper
{
    public static DoctorDegree ToDomain(VisionCare.Infrastructure.Models.Degreedoctor model)
    {
        return new DoctorDegree
        {
            Id = 0, // Composite key, will be handled differently
            DoctorId = model.DoctorId,
            DegreeId = model.DegreeId,
            IssuedDate = model.IssuedDate,
            IssuedBy = model.IssuedBy,
            CertificateImage = model.CertificateImage,
            Status = model.Status,
            Created = DateTime.UtcNow,
            LastModified = DateTime.UtcNow
        };
    }

    public static VisionCare.Infrastructure.Models.Degreedoctor ToInfrastructure(DoctorDegree domain)
    {
        return new VisionCare.Infrastructure.Models.Degreedoctor
        {
            DoctorId = domain.DoctorId,
            DegreeId = domain.DegreeId,
            IssuedDate = domain.IssuedDate,
            IssuedBy = domain.IssuedBy,
            CertificateImage = domain.CertificateImage,
            Status = domain.Status
        };
    }
}
