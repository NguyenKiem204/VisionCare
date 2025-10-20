using FluentValidation;
using VisionCare.Application.DTOs.SpecializationDto;

namespace VisionCare.Application.Validators;

public class CreateSpecializationRequestValidator : AbstractValidator<CreateSpecializationRequest>
{
    public CreateSpecializationRequestValidator()
    {
        RuleFor(x => x.SpecializationName)
            .NotEmpty()
            .WithMessage("Specialization name is required")
            .MaximumLength(255)
            .WithMessage("Specialization name cannot exceed 255 characters")
            .Matches(@"^[a-zA-Z\s]+$")
            .WithMessage("Specialization name can only contain letters and spaces");
    }
}

public class UpdateSpecializationRequestValidator : AbstractValidator<UpdateSpecializationRequest>
{
    public UpdateSpecializationRequestValidator()
    {
        RuleFor(x => x.SpecializationName)
            .NotEmpty()
            .WithMessage("Specialization name is required")
            .MaximumLength(255)
            .WithMessage("Specialization name cannot exceed 255 characters")
            .Matches(@"^[a-zA-Z\s]+$")
            .WithMessage("Specialization name can only contain letters and spaces");

        RuleFor(x => x.SpecializationStatus)
            .Must(status => status == null || status == "Active" || status == "Inactive")
            .WithMessage("Specialization status must be Active or Inactive");
    }
}
