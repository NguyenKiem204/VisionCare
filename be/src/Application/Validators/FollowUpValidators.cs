using FluentValidation;
using VisionCare.Application.DTOs.FollowUpDto;

namespace VisionCare.Application.Validators;

public class CreateFollowUpRequestValidator : AbstractValidator<CreateFollowUpRequest>
{
    public CreateFollowUpRequestValidator()
    {
        RuleFor(x => x.AppointmentId)
            .GreaterThan(0)
            .WithMessage("Appointment ID must be greater than 0");

        RuleFor(x => x.NextAppointmentDate)
            .GreaterThan(DateTime.UtcNow.AddMinutes(30))
            .WithMessage("Next appointment date must be at least 30 minutes in advance")
            .LessThan(DateTime.UtcNow.AddMonths(6))
            .WithMessage("Next appointment date cannot be more than 6 months in advance");

        RuleFor(x => x.Description)
            .MaximumLength(500)
            .WithMessage("Description cannot exceed 500 characters")
            .When(x => !string.IsNullOrEmpty(x.Description));
    }
}

public class UpdateFollowUpRequestValidator : AbstractValidator<UpdateFollowUpRequest>
{
    public UpdateFollowUpRequestValidator()
    {
        RuleFor(x => x.NextAppointmentDate)
            .GreaterThan(DateTime.UtcNow.AddMinutes(30))
            .WithMessage("Next appointment date must be at least 30 minutes in advance")
            .LessThan(DateTime.UtcNow.AddMonths(6))
            .WithMessage("Next appointment date cannot be more than 6 months in advance")
            .When(x => x.NextAppointmentDate.HasValue);

        RuleFor(x => x.Description)
            .MaximumLength(500)
            .WithMessage("Description cannot exceed 500 characters")
            .When(x => !string.IsNullOrEmpty(x.Description));

        RuleFor(x => x.Status)
            .Must(status =>
                status == null
                || new[]
                {
                    "Pending",
                    "Scheduled",
                    "Completed",
                    "Cancelled",
                    "Rescheduled",
                }.Contains(status)
            )
            .WithMessage(
                "Status must be one of: Pending, Scheduled, Completed, Cancelled, Rescheduled"
            )
            .When(x => !string.IsNullOrEmpty(x.Status));
    }
}
