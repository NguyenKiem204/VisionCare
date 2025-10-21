using FluentValidation;
using VisionCare.Application.DTOs.AppointmentDto;

namespace VisionCare.Application.Validators;

public class RescheduleAppointmentRequestValidator : AbstractValidator<RescheduleAppointmentRequest>
{
    public RescheduleAppointmentRequestValidator()
    {
        RuleFor(x => x.NewAppointmentDate)
            .GreaterThan(DateTime.UtcNow.AddMinutes(30))
            .WithMessage("Appointment must be scheduled at least 30 minutes in advance")
            .LessThan(DateTime.UtcNow.AddMonths(3))
            .WithMessage("Appointment cannot be scheduled more than 3 months in advance");

        RuleFor(x => x.NewAppointmentDate)
            .Must(date => date.Hour >= 8 && date.Hour <= 17)
            .WithMessage("Appointment must be scheduled between 8 AM and 5 PM");

        RuleFor(x => x.NewAppointmentDate)
            .Must(date => date.DayOfWeek != DayOfWeek.Sunday)
            .WithMessage("Appointments cannot be scheduled on Sundays");

        RuleFor(x => x.Reason)
            .MaximumLength(500)
            .WithMessage("Reason cannot exceed 500 characters")
            .When(x => !string.IsNullOrEmpty(x.Reason));
    }
}
