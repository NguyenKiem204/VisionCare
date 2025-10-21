using DomainAppointment = VisionCare.Domain.Entities.Appointment;

namespace VisionCare.Infrastructure.Mappings;

public static class AppointmentMapper
{
    public static DomainAppointment ToDomain(VisionCare.Infrastructure.Models.Appointment model)
    {
        return new DomainAppointment
        {
            Id = model.AppointmentId,
            AppointmentDate = model.AppointmentDatetime,
            AppointmentStatus = model.Status,
            DoctorId = model.DoctorId,
            PatientId = model.PatientId,
            Notes = model.Notes,
            Created = model.CreatedAt ?? DateTime.UtcNow,
            LastModified = DateTime.UtcNow,
            Doctor = model.Doctor != null ? DoctorMapper.ToDomain(model.Doctor) : null,
            Patient = model.Patient != null ? CustomerMapper.ToDomain(model.Patient) : null,
        };
    }

    public static VisionCare.Infrastructure.Models.Appointment ToInfrastructure(
        DomainAppointment domain
    )
    {
        return new VisionCare.Infrastructure.Models.Appointment
        {
            AppointmentId = domain.Id,
            AppointmentDatetime = domain.AppointmentDate ?? DateTime.UtcNow,
            Status = domain.AppointmentStatus ?? "Pending",
            DoctorId = domain.DoctorId ?? 0,
            PatientId = domain.PatientId ?? 0,
            Notes = domain.Notes,
            CreatedAt = domain.Created,
        };
    }
}
