using FluentValidation;
using VisionCare.Application.DTOs.ServiceDto;

namespace VisionCare.Application.Validators;

public class CreateServiceRequestValidator : AbstractValidator<CreateServiceRequest>
{
    public CreateServiceRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Service name is required")
            .MaximumLength(255)
            .WithMessage("Service name cannot exceed 255 characters");

        RuleFor(x => x.Description)
            .MaximumLength(1000)
            .WithMessage("Description cannot exceed 1000 characters")
            .When(x => !string.IsNullOrEmpty(x.Description));

        RuleFor(x => x.Benefits)
            .MaximumLength(1000)
            .WithMessage("Benefits cannot exceed 1000 characters")
            .When(x => !string.IsNullOrEmpty(x.Benefits));

        RuleFor(x => x.SpecializationId)
            .GreaterThan(0)
            .WithMessage("Specialization ID must be greater than 0")
            .When(x => x.SpecializationId.HasValue);
    }
}

public class UpdateServiceRequestValidator : AbstractValidator<UpdateServiceRequest>
{
    public UpdateServiceRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Service name is required")
            .MaximumLength(255)
            .WithMessage("Service name cannot exceed 255 characters");

        RuleFor(x => x.Description)
            .MaximumLength(1000)
            .WithMessage("Description cannot exceed 1000 characters")
            .When(x => !string.IsNullOrEmpty(x.Description));

        RuleFor(x => x.Benefits)
            .MaximumLength(1000)
            .WithMessage("Benefits cannot exceed 1000 characters")
            .When(x => !string.IsNullOrEmpty(x.Benefits));

        RuleFor(x => x.SpecializationId)
            .GreaterThan(0)
            .WithMessage("Specialization ID must be greater than 0")
            .When(x => x.SpecializationId.HasValue);
    }
}
