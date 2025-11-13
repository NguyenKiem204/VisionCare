using VisionCare.Domain.Entities;

namespace VisionCare.Infrastructure.Mappings;

public static class DoctorDegreeMapper
{
    public static DoctorDegree ToDomain(VisionCare.Infrastructure.Models.Degreedoctor model)
    {
        return new DoctorDegree
        {
            Id = model.DegreeId,
            DoctorId = model.DoctorId,
            DegreeId = model.DegreeId,
            IssuedDate = model.IssuedDate,
            IssuedBy = model.IssuedBy,
            CertificateImage = model.CertificateImage,
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
            Degree = model.Degree != null
                ? new Degree
                {
                    Id = model.Degree.DegreeId,
                    Name = model.Degree.Name
                }
                : null
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
