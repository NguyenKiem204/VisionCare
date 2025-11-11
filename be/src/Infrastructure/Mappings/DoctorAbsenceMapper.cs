using VisionCare.Domain.Entities;

namespace VisionCare.Infrastructure.Mappings;

public static class DoctorAbsenceMapper
{
    public static DoctorAbsence ToDomain(VisionCare.Infrastructure.Models.Doctorabsence model)
    {
        return new DoctorAbsence
        {
            Id = model.AbsenceId,
            DoctorId = model.DoctorId,
            StartDate = model.StartDate,
            EndDate = model.EndDate,
            AbsenceType = model.AbsenceType ?? "Leave",
            Reason = model.Reason ?? string.Empty,
            Status = model.Status ?? "Pending",
            IsResolved = model.IsResolved ?? false,
            Created = model.CreatedAt ?? DateTime.UtcNow,
            LastModified = model.UpdatedAt ?? DateTime.UtcNow,
        };
    }

    public static VisionCare.Infrastructure.Models.Doctorabsence ToInfrastructure(
        DoctorAbsence domain
    )
    {
        return new VisionCare.Infrastructure.Models.Doctorabsence
        {
            AbsenceId = domain.Id,
            DoctorId = domain.DoctorId,
            StartDate = domain.StartDate,
            EndDate = domain.EndDate,
            AbsenceType = domain.AbsenceType,
            Reason = domain.Reason,
            Status = domain.Status,
            IsResolved = domain.IsResolved,
            CreatedAt = domain.Created,
            UpdatedAt = domain.LastModified,
        };
    }
}
