using VisionCare.Domain.Entities;

namespace VisionCare.Infrastructure.Mappings;

public static class DoctorCertificateMapper
{
    public static DoctorCertificate ToDomain(VisionCare.Infrastructure.Models.Certificatedoctor model)
    {
        return new DoctorCertificate
        {
            Id = model.CertificateId,
            DoctorId = model.DoctorId,
            CertificateId = model.CertificateId,
            IssuedDate = model.IssuedDate,
            IssuedBy = model.IssuedBy,
            CertificateImage = model.CertificateImage,
            ExpiryDate = model.ExpiryDate,
            Status = model.Status,
            Created = DateTime.UtcNow,
            LastModified = DateTime.UtcNow,
            Doctor = model.Doctor != null
                ? new Doctor
                {
                    Id = model.Doctor.AccountId,
                    DoctorName = model.Doctor.FullName,
                    ProfileImage = model.Doctor.Avatar,
                    ExperienceYears = model.Doctor.ExperienceYears,
                    SpecializationId = model.Doctor.SpecializationId,
                    Gender = model.Doctor.Gender,
                    Dob = model.Doctor.Dob,
                    Address = model.Doctor.Address,
                    Phone = model.Doctor.Phone
                }
                : null,
            Certificate = model.Certificate != null
                ? new Certificate
                {
                    Id = model.Certificate.CertificateId,
                    Name = model.Certificate.Name
                }
                : null
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
