using FluentValidation;
using VisionCare.Application.DTOs.DoctorDto;

namespace VisionCare.Application.Validators;

public class CreateDoctorRequestValidator : AbstractValidator<CreateDoctorRequest>
{
    public CreateDoctorRequestValidator()
    {
        RuleFor(x => x.AccountId).GreaterThan(0).WithMessage("Account ID must be greater than 0");

        RuleFor(x => x.DoctorName)
            .NotEmpty()
            .WithMessage("Doctor name is required")
            .MaximumLength(255)
            .WithMessage("Doctor name cannot exceed 255 characters");

        RuleFor(x => x.ExperienceYears)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Experience years must be 0 or greater")
            .LessThanOrEqualTo(50)
            .WithMessage("Experience years cannot exceed 50");

        RuleFor(x => x.SpecializationId)
            .GreaterThan(0)
            .WithMessage("Specialization ID must be greater than 0")
            .When(x => x.SpecializationId.HasValue);

        RuleFor(x => x.Gender)
            .Must(gender =>
                gender == null || gender == "Male" || gender == "Female" || gender == "Other"
            )
            .WithMessage("Gender must be Male, Female, or Other");

        RuleFor(x => x.Dob)
            .LessThan(DateOnly.FromDateTime(DateTime.Today.AddYears(-18)))
            .WithMessage("Doctor must be at least 18 years old")
            .When(x => x.Dob.HasValue);

        RuleFor(x => x.Address)
            .MaximumLength(500)
            .WithMessage("Address cannot exceed 500 characters");
    }
}

public class UpdateDoctorRequestValidator : AbstractValidator<UpdateDoctorRequest>
{
    public UpdateDoctorRequestValidator()
    {
        RuleFor(x => x.DoctorName)
            .NotEmpty()
            .WithMessage("Doctor name is required")
            .MaximumLength(255)
            .WithMessage("Doctor name cannot exceed 255 characters");

        RuleFor(x => x.ExperienceYears)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Experience years must be 0 or greater")
            .LessThanOrEqualTo(50)
            .WithMessage("Experience years cannot exceed 50");

        RuleFor(x => x.SpecializationId)
            .GreaterThan(0)
            .WithMessage("Specialization ID must be greater than 0")
            .When(x => x.SpecializationId.HasValue);

        RuleFor(x => x.Gender)
            .Must(gender =>
                gender == null || gender == "Male" || gender == "Female" || gender == "Other"
            )
            .WithMessage("Gender must be Male, Female, or Other");

        RuleFor(x => x.Dob)
            .LessThan(DateOnly.FromDateTime(DateTime.Today.AddYears(-18)))
            .WithMessage("Doctor must be at least 18 years old")
            .When(x => x.Dob.HasValue);

        RuleFor(x => x.Address)
            .MaximumLength(500)
            .WithMessage("Address cannot exceed 500 characters");
    }
}
