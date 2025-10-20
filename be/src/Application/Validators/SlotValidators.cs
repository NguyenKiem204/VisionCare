using FluentValidation;
using VisionCare.Application.DTOs.SlotDto;

namespace VisionCare.Application.Validators;

public class CreateSlotRequestValidator : AbstractValidator<CreateSlotRequest>
{
    public CreateSlotRequestValidator()
    {
        RuleFor(x => x.StartTime)
            .LessThan(x => x.EndTime)
            .WithMessage("Start time must be before end time");

        RuleFor(x => x.EndTime)
            .GreaterThan(x => x.StartTime)
            .WithMessage("End time must be after start time");

        RuleFor(x => x.ServiceTypeId)
            .GreaterThan(0)
            .WithMessage("Service Type ID must be greater than 0");

        RuleFor(x => x.StartTime)
            .Must(startTime => startTime.Hour >= 8 && startTime.Hour <= 17)
            .WithMessage("Start time must be between 8 AM and 5 PM");

        RuleFor(x => x.EndTime)
            .Must(endTime => endTime.Hour >= 9 && endTime.Hour <= 18)
            .WithMessage("End time must be between 9 AM and 6 PM");
    }
}

public class UpdateSlotRequestValidator : AbstractValidator<UpdateSlotRequest>
{
    public UpdateSlotRequestValidator()
    {
        RuleFor(x => x.StartTime)
            .LessThan(x => x.EndTime)
            .WithMessage("Start time must be before end time");

        RuleFor(x => x.EndTime)
            .GreaterThan(x => x.StartTime)
            .WithMessage("End time must be after start time");

        RuleFor(x => x.StartTime)
            .Must(startTime => startTime.Hour >= 8 && startTime.Hour <= 17)
            .WithMessage("Start time must be between 8 AM and 5 PM");

        RuleFor(x => x.EndTime)
            .Must(endTime => endTime.Hour >= 9 && endTime.Hour <= 18)
            .WithMessage("End time must be between 9 AM and 6 PM");
    }
}
