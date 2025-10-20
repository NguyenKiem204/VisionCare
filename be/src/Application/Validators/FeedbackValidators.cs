using FluentValidation;
using VisionCare.Application.DTOs.FeedbackDto;

namespace VisionCare.Application.Validators;

public class CreateFeedbackDoctorRequestValidator : AbstractValidator<CreateFeedbackDoctorRequest>
{
    public CreateFeedbackDoctorRequestValidator()
    {
        RuleFor(x => x.AppointmentId)
            .GreaterThan(0)
            .WithMessage("Appointment ID must be greater than 0");

        RuleFor(x => x.Rating).InclusiveBetween(1, 5).WithMessage("Rating must be between 1 and 5");

        RuleFor(x => x.FeedbackText)
            .MaximumLength(1000)
            .WithMessage("Feedback text cannot exceed 1000 characters")
            .When(x => !string.IsNullOrEmpty(x.FeedbackText));
    }
}

public class UpdateFeedbackDoctorRequestValidator : AbstractValidator<UpdateFeedbackDoctorRequest>
{
    public UpdateFeedbackDoctorRequestValidator()
    {
        RuleFor(x => x.Rating).InclusiveBetween(1, 5).WithMessage("Rating must be between 1 and 5");

        RuleFor(x => x.FeedbackText)
            .MaximumLength(1000)
            .WithMessage("Feedback text cannot exceed 1000 characters")
            .When(x => !string.IsNullOrEmpty(x.FeedbackText));
    }
}

public class RespondToFeedbackRequestValidator : AbstractValidator<RespondToFeedbackRequest>
{
    public RespondToFeedbackRequestValidator()
    {
        RuleFor(x => x.ResponseText)
            .NotEmpty()
            .WithMessage("Response text is required")
            .MaximumLength(1000)
            .WithMessage("Response text cannot exceed 1000 characters");
    }
}

public class CreateFeedbackServiceRequestValidator : AbstractValidator<CreateFeedbackServiceRequest>
{
    public CreateFeedbackServiceRequestValidator()
    {
        RuleFor(x => x.AppointmentId)
            .GreaterThan(0)
            .WithMessage("Appointment ID must be greater than 0");

        RuleFor(x => x.Rating).InclusiveBetween(1, 5).WithMessage("Rating must be between 1 and 5");

        RuleFor(x => x.FeedbackText)
            .MaximumLength(1000)
            .WithMessage("Feedback text cannot exceed 1000 characters")
            .When(x => !string.IsNullOrEmpty(x.FeedbackText));
    }
}

public class UpdateFeedbackServiceRequestValidator : AbstractValidator<UpdateFeedbackServiceRequest>
{
    public UpdateFeedbackServiceRequestValidator()
    {
        RuleFor(x => x.Rating).InclusiveBetween(1, 5).WithMessage("Rating must be between 1 and 5");

        RuleFor(x => x.FeedbackText)
            .MaximumLength(1000)
            .WithMessage("Feedback text cannot exceed 1000 characters")
            .When(x => !string.IsNullOrEmpty(x.FeedbackText));
    }
}

public class RespondToServiceFeedbackRequestValidator
    : AbstractValidator<RespondToServiceFeedbackRequest>
{
    public RespondToServiceFeedbackRequestValidator()
    {
        RuleFor(x => x.ResponseText)
            .NotEmpty()
            .WithMessage("Response text is required")
            .MaximumLength(1000)
            .WithMessage("Response text cannot exceed 1000 characters");
    }
}
