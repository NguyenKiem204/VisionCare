using FluentValidation;
using VisionCare.Application.DTOs.BookingDto;

namespace VisionCare.Application.Validators;

public class HoldSlotRequestValidator : AbstractValidator<HoldSlotRequest>
{
    public HoldSlotRequestValidator()
    {
        RuleFor(x => x.DoctorId).GreaterThan(0).WithMessage("Doctor ID is required");
        RuleFor(x => x.SlotId).GreaterThan(0).WithMessage("Slot ID is required");
        RuleFor(x => x.ScheduleDate)
            .NotEmpty()
            .WithMessage("Schedule date is required")
            .Must(date => date >= DateOnly.FromDateTime(DateTime.Today))
            .WithMessage("Schedule date cannot be in the past")
            .Must(date => date <= DateOnly.FromDateTime(DateTime.Today.AddDays(30)))
            .WithMessage("Schedule date cannot be more than 30 days ahead");
    }
}

public class CreateBookingRequestValidator : AbstractValidator<CreateBookingRequest>
{
    public CreateBookingRequestValidator()
    {
        RuleFor(x => x.DoctorId).GreaterThan(0).WithMessage("Doctor ID is required");
        RuleFor(x => x.ServiceDetailId).GreaterThan(0).WithMessage("Service detail ID is required");
        RuleFor(x => x.SlotId).GreaterThan(0).WithMessage("Slot ID is required");

        RuleFor(x => x.ScheduleDate)
            .Must(date => date != default(DateOnly))
            .WithMessage("Schedule date is required")
            .Must(date => date >= DateOnly.FromDateTime(DateTime.Today))
            .WithMessage("Schedule date cannot be in the past")
            .Must(date => date <= DateOnly.FromDateTime(DateTime.Today.AddDays(30)))
            .WithMessage("Schedule date cannot be more than 30 days ahead");

        RuleFor(x => x.StartTime)
            .NotEmpty()
            .WithMessage("Start time is required")
            .Must(time => time != default(TimeOnly))
            .WithMessage("Start time is required");

        RuleFor(x => x)
            .Must(x =>
                x.CustomerId.HasValue
                || (!string.IsNullOrEmpty(x.Phone) || !string.IsNullOrEmpty(x.Email))
            )
            .WithMessage("Either Customer ID or (Phone/Email) must be provided");

        When(
            x => !x.CustomerId.HasValue,
            () =>
            {
                RuleFor(x => x.Email)
                    .NotEmpty()
                    .WithMessage("Email is required when Customer ID is not provided")
                    .EmailAddress()
                    .WithMessage("Invalid email format");

                RuleFor(x => x.CustomerName)
                    .NotEmpty()
                    .WithMessage("Customer name is required when Customer ID is not provided");
            }
        );

        When(
            x => !string.IsNullOrEmpty(x.Phone),
            () =>
            {
                RuleFor(x => x.Phone)
                    .Matches(@"^(0|\+84)[3-9][0-9]{8,9}$")
                    .WithMessage(
                        "Invalid Vietnamese phone number format. Expected: 0xxxxxxxxx or +84xxxxxxxxx (9-10 digits after prefix)"
                    );
            }
        );
    }
}
