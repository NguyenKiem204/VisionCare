using FluentValidation;
using VisionCare.Application.DTOs.ScheduleDto;

namespace VisionCare.Application.Validators;

public class CreateScheduleRequestValidator : AbstractValidator<CreateScheduleRequest>
{
    public CreateScheduleRequestValidator()
    {
        RuleFor(x => x.DoctorId).GreaterThan(0).WithMessage("Doctor ID must be greater than 0");

        RuleFor(x => x.SlotId).GreaterThan(0).WithMessage("Slot ID must be greater than 0");

        RuleFor(x => x.ScheduleDate)
            .GreaterThanOrEqualTo(DateOnly.FromDateTime(DateTime.Today))
            .WithMessage("Schedule date cannot be in the past")
            .LessThanOrEqualTo(DateOnly.FromDateTime(DateTime.Today.AddMonths(3)))
            .WithMessage("Schedule date cannot be more than 3 months in advance");
    }
}

public class UpdateScheduleRequestValidator : AbstractValidator<UpdateScheduleRequest>
{
    public UpdateScheduleRequestValidator()
    {
        RuleFor(x => x.Status)
            .Must(status =>
                status == null || new[] { "Available", "Booked", "Blocked" }.Contains(status)
            )
            .WithMessage("Status must be one of: Available, Booked, Blocked")
            .When(x => !string.IsNullOrEmpty(x.Status));
    }
}

public class AvailableSlotsRequestValidator : AbstractValidator<AvailableSlotsRequest>
{
    public AvailableSlotsRequestValidator()
    {
        RuleFor(x => x.DoctorId).GreaterThan(0).WithMessage("Doctor ID must be greater than 0");

        RuleFor(x => x.ScheduleDate)
            .GreaterThanOrEqualTo(DateOnly.FromDateTime(DateTime.Today))
            .WithMessage("Schedule date cannot be in the past")
            .LessThanOrEqualTo(DateOnly.FromDateTime(DateTime.Today.AddMonths(3)))
            .WithMessage("Schedule date cannot be more than 3 months in advance");

        RuleFor(x => x.ServiceTypeId)
            .GreaterThan(0)
            .WithMessage("Service Type ID must be greater than 0")
            .When(x => x.ServiceTypeId.HasValue);
    }
}
