using VisionCare.Domain.Entities;

namespace VisionCare.Infrastructure.Mappings;

public static class DoctorMapper
{
    public static Doctor ToDomain(VisionCare.Infrastructure.Models.Doctor model)
    {
        return new Doctor
        {
            Id = model.AccountId,
            AccountId = model.AccountId,
            DoctorName = model.FullName,
            Phone = model.Phone,
            ExperienceYears = model.ExperienceYears,
            SpecializationId = model.SpecializationId,
            ProfileImage = model.Avatar,
            Rating = model.Rating.HasValue ? (double?)model.Rating.Value : null,
            Gender = model.Gender,
            Dob = model.Dob,
            Address = model.Address,
            DoctorStatus = model.Status,
            Biography = model.Biography,
            Created = DateTime.UtcNow,
            LastModified = DateTime.UtcNow,
            Account =
                model.Account != null
                    ? new User
                    {
                        Id = model.Account.AccountId,
                        Email = model.Account.Email,
                        Username = model.Account.Username,
                    }
                    : null,
            Specialization =
                model.Specialization != null
                    ? new Specialization
                    {
                        Id = model.Specialization.SpecializationId,
                        SpecializationName = model.Specialization.Name,
                    }
                    : null,
        };
    }

    public static VisionCare.Infrastructure.Models.Doctor ToInfrastructure(Doctor domain)
    {
        return new VisionCare.Infrastructure.Models.Doctor
        {
            AccountId = domain.AccountId ?? 0,
            FullName = domain.DoctorName,
            Phone = domain.Phone,
            ExperienceYears = domain.ExperienceYears.HasValue
                ? (short?)domain.ExperienceYears.Value
                : null,
            SpecializationId = domain.SpecializationId ?? 0,
            Avatar = domain.ProfileImage,
            Rating = domain.Rating.HasValue ? (decimal?)domain.Rating.Value : null,
            Gender = domain.Gender,
            Dob = domain.Dob,
            Address = domain.Address,
            Status = domain.DoctorStatus ?? "Active",
            Biography = domain.Biography,
        };
    }
}
