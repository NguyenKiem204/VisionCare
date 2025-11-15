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

public class RequestRescheduleRequestValidator : AbstractValidator<RequestRescheduleRequest>
{
    public RequestRescheduleRequestValidator()
    {
        RuleFor(x => x.ProposedDateTime)
            .NotEmpty()
            .WithMessage("Thời gian đề xuất là bắt buộc")
            .GreaterThan(DateTime.UtcNow.AddHours(24))
            .WithMessage("Thời gian đề xuất phải cách ít nhất 24 giờ")
            .LessThan(DateTime.UtcNow.AddMonths(3))
            .WithMessage("Thời gian đề xuất không được quá 3 tháng");

        RuleFor(x => x.Reason)
            .NotEmpty()
            .WithMessage("Lý do đổi lịch là bắt buộc")
            .MinimumLength(5)
            .WithMessage("Lý do phải có ít nhất 5 ký tự")
            .MaximumLength(500)
            .WithMessage("Lý do không được vượt quá 500 ký tự");
    }
}

public class RejectRescheduleRequestValidator : AbstractValidator<RejectRescheduleRequest>
{
    public RejectRescheduleRequestValidator()
    {
        RuleFor(x => x.Reason)
            .MaximumLength(500)
            .WithMessage("Reason cannot exceed 500 characters")
            .When(x => !string.IsNullOrEmpty(x.Reason));
    }
}

public class CounterRescheduleRequestValidator : AbstractValidator<CounterRescheduleRequest>
{
    public CounterRescheduleRequestValidator()
    {
        RuleFor(x => x.ProposedDateTime)
            .NotEmpty()
            .WithMessage("Thời gian đề xuất là bắt buộc")
            .GreaterThan(DateTime.UtcNow.AddHours(24))
            .WithMessage("Thời gian đề xuất phải cách ít nhất 24 giờ")
            .LessThan(DateTime.UtcNow.AddMonths(3))
            .WithMessage("Thời gian đề xuất không được quá 3 tháng");

        RuleFor(x => x.Reason)
            .NotEmpty()
            .WithMessage("Lý do đề xuất thời gian khác là bắt buộc")
            .MinimumLength(5)
            .WithMessage("Lý do phải có ít nhất 5 ký tự")
            .MaximumLength(500)
            .WithMessage("Lý do không được vượt quá 500 ký tự");
    }
}