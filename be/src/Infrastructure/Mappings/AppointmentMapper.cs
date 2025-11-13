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
            AppointmentCode = model.AppointmentCode,
            PaymentStatus = model.PaymentStatus,
            ActualCost = model.ActualCost,
            ServiceDetailId = model.ServiceDetailId,
            DiscountId = model.DiscountId,
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
        // Convert DateTime to Unspecified kind for PostgreSQL timestamp without time zone
        var appointmentDatetime = domain.AppointmentDate.HasValue
            ? (domain.AppointmentDate.Value.Kind == DateTimeKind.Unspecified
                ? domain.AppointmentDate.Value
                : DateTime.SpecifyKind(domain.AppointmentDate.Value, DateTimeKind.Unspecified))
            : DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Unspecified);

        var createdAt = domain.Created.Kind == DateTimeKind.Unspecified
            ? domain.Created
            : DateTime.SpecifyKind(domain.Created, DateTimeKind.Unspecified);

        return new VisionCare.Infrastructure.Models.Appointment
        {
            AppointmentId = domain.Id,
            AppointmentDatetime = appointmentDatetime,
            Status = domain.AppointmentStatus ?? "Pending",
            DoctorId = domain.DoctorId ?? 0,
            PatientId = domain.PatientId ?? 0,
            ServiceDetailId = domain.ServiceDetailId ?? 0,
            DiscountId = domain.DiscountId,
            Notes = domain.Notes,
            AppointmentCode = domain.AppointmentCode,
            PaymentStatus = domain.PaymentStatus ?? "Unpaid",
            ActualCost = domain.ActualCost,
            CreatedAt = createdAt,
        };
    }
}
