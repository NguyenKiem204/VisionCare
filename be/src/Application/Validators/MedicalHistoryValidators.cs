using FluentValidation;
using VisionCare.Application.DTOs.MedicalHistoryDto;

namespace VisionCare.Application.Validators;

public class CreateMedicalHistoryRequestValidator : AbstractValidator<CreateMedicalHistoryRequest>
{
    public CreateMedicalHistoryRequestValidator()
    {
        RuleFor(x => x.AppointmentId)
            .GreaterThan(0)
            .WithMessage("Appointment ID must be greater than 0");

        RuleFor(x => x.Diagnosis)
            .MaximumLength(1000)
            .WithMessage("Diagnosis cannot exceed 1000 characters")
            .When(x => !string.IsNullOrEmpty(x.Diagnosis));

        RuleFor(x => x.Symptoms)
            .MaximumLength(1000)
            .WithMessage("Symptoms cannot exceed 1000 characters")
            .When(x => !string.IsNullOrEmpty(x.Symptoms));

        RuleFor(x => x.Treatment)
            .MaximumLength(1000)
            .WithMessage("Treatment cannot exceed 1000 characters")
            .When(x => !string.IsNullOrEmpty(x.Treatment));

        RuleFor(x => x.Prescription)
            .MaximumLength(1000)
            .WithMessage("Prescription cannot exceed 1000 characters")
            .When(x => !string.IsNullOrEmpty(x.Prescription));

        RuleFor(x => x.VisionLeft)
            .InclusiveBetween(0.0m, 2.0m)
            .WithMessage("Left vision must be between 0.0 and 2.0")
            .When(x => x.VisionLeft.HasValue);

        RuleFor(x => x.VisionRight)
            .InclusiveBetween(0.0m, 2.0m)
            .WithMessage("Right vision must be between 0.0 and 2.0")
            .When(x => x.VisionRight.HasValue);

        RuleFor(x => x.AdditionalTests)
            .MaximumLength(1000)
            .WithMessage("Additional tests cannot exceed 1000 characters")
            .When(x => !string.IsNullOrEmpty(x.AdditionalTests));

        RuleFor(x => x.Notes)
            .MaximumLength(1000)
            .WithMessage("Notes cannot exceed 1000 characters")
            .When(x => !string.IsNullOrEmpty(x.Notes));
    }
}

public class UpdateMedicalHistoryRequestValidator : AbstractValidator<UpdateMedicalHistoryRequest>
{
    public UpdateMedicalHistoryRequestValidator()
    {
        RuleFor(x => x.Diagnosis)
            .MaximumLength(1000)
            .WithMessage("Diagnosis cannot exceed 1000 characters")
            .When(x => !string.IsNullOrEmpty(x.Diagnosis));

        RuleFor(x => x.Symptoms)
            .MaximumLength(1000)
            .WithMessage("Symptoms cannot exceed 1000 characters")
            .When(x => !string.IsNullOrEmpty(x.Symptoms));

        RuleFor(x => x.Treatment)
            .MaximumLength(1000)
            .WithMessage("Treatment cannot exceed 1000 characters")
            .When(x => !string.IsNullOrEmpty(x.Treatment));

        RuleFor(x => x.Prescription)
            .MaximumLength(1000)
            .WithMessage("Prescription cannot exceed 1000 characters")
            .When(x => !string.IsNullOrEmpty(x.Prescription));

        RuleFor(x => x.VisionLeft)
            .InclusiveBetween(0.0m, 2.0m)
            .WithMessage("Left vision must be between 0.0 and 2.0")
            .When(x => x.VisionLeft.HasValue);

        RuleFor(x => x.VisionRight)
            .InclusiveBetween(0.0m, 2.0m)
            .WithMessage("Right vision must be between 0.0 and 2.0")
            .When(x => x.VisionRight.HasValue);

        RuleFor(x => x.AdditionalTests)
            .MaximumLength(1000)
            .WithMessage("Additional tests cannot exceed 1000 characters")
            .When(x => !string.IsNullOrEmpty(x.AdditionalTests));

        RuleFor(x => x.Notes)
            .MaximumLength(1000)
            .WithMessage("Notes cannot exceed 1000 characters")
            .When(x => !string.IsNullOrEmpty(x.Notes));
    }
}
