using FluentValidation;
using VisionCare.Application.DTOs.AppointmentDto;

namespace VisionCare.Application.Validators;

public class CreateAppointmentRequestValidator : AbstractValidator<CreateAppointmentRequest>
{
    public CreateAppointmentRequestValidator()
    {
        RuleFor(x => x.DoctorId)
            .GreaterThan(0)
            .WithMessage("Doctor ID must be greater than 0");

        RuleFor(x => x.PatientId)
            .GreaterThan(0)
            .WithMessage("Patient ID must be greater than 0");

        RuleFor(x => x.AppointmentDate)
            .GreaterThan(DateTime.UtcNow.AddMinutes(30))
            .WithMessage("Appointment must be scheduled at least 30 minutes in advance")
            .LessThan(DateTime.UtcNow.AddMonths(3))
            .WithMessage("Appointment cannot be scheduled more than 3 months in advance");

        RuleFor(x => x.AppointmentDate)
            .Must(date => date.Hour >= 8 && date.Hour <= 17)
            .WithMessage("Appointment must be scheduled between 8 AM and 5 PM");

        RuleFor(x => x.AppointmentDate)
            .Must(date => date.DayOfWeek != DayOfWeek.Sunday)
            .WithMessage("Appointments cannot be scheduled on Sundays");

        RuleFor(x => x.Notes)
            .MaximumLength(1000)
            .WithMessage("Notes cannot exceed 1000 characters");
    }
}

public class UpdateAppointmentRequestValidator : AbstractValidator<UpdateAppointmentRequest>
{
    public UpdateAppointmentRequestValidator()
    {
        RuleFor(x => x.AppointmentDate)
            .GreaterThan(DateTime.UtcNow.AddMinutes(30))
            .WithMessage("Appointment must be scheduled at least 30 minutes in advance")
            .LessThan(DateTime.UtcNow.AddMonths(3))
            .WithMessage("Appointment cannot be scheduled more than 3 months in advance")
            .When(x => x.AppointmentDate.HasValue);

        RuleFor(x => x.AppointmentDate)
            .Must(date => date.HasValue && date.Value.Hour >= 8 && date.Value.Hour <= 17)
            .WithMessage("Appointment must be scheduled between 8 AM and 5 PM")
            .When(x => x.AppointmentDate.HasValue);

        RuleFor(x => x.AppointmentDate)
            .Must(date => date.HasValue && date.Value.DayOfWeek != DayOfWeek.Sunday)
            .WithMessage("Appointments cannot be scheduled on Sundays")
            .When(x => x.AppointmentDate.HasValue);

        RuleFor(x => x.AppointmentStatus)
            .Must(status => status == null || 
                status == "Pending" || 
                status == "Confirmed" || 
                status == "Cancelled" || 
                status == "Completed" || 
                status == "Rescheduled")
            .WithMessage("Invalid appointment status");

        RuleFor(x => x.Notes)
            .MaximumLength(1000)
            .WithMessage("Notes cannot exceed 1000 characters");
    }
}
