using FluentValidation;
using VisionCare.Application.DTOs.StaffDto;

namespace VisionCare.Application.Validators;

public class CreateStaffRequestValidator : AbstractValidator<CreateStaffRequest>
{
    public CreateStaffRequestValidator()
    {
        RuleFor(x => x.AccountId)
            .GreaterThan(0)
            .WithMessage("Account ID must be greater than 0");

        RuleFor(x => x.StaffName)
            .NotEmpty()
            .WithMessage("Staff name is required")
            .MaximumLength(255)
            .WithMessage("Staff name cannot exceed 255 characters");

        RuleFor(x => x.Gender)
            .Must(gender => gender == null || gender == "Male" || gender == "Female" || gender == "Other")
            .WithMessage("Gender must be Male, Female, or Other");

        RuleFor(x => x.Dob)
            .LessThan(DateOnly.FromDateTime(DateTime.Today.AddYears(-18)))
            .WithMessage("Staff must be at least 18 years old")
            .When(x => x.Dob.HasValue);

        RuleFor(x => x.Dob)
            .GreaterThan(DateOnly.FromDateTime(DateTime.Today.AddYears(-65)))
            .WithMessage("Staff cannot be older than 65 years")
            .When(x => x.Dob.HasValue);

        RuleFor(x => x.Address)
            .MaximumLength(500)
            .WithMessage("Address cannot exceed 500 characters");
    }
}

public class UpdateStaffRequestValidator : AbstractValidator<UpdateStaffRequest>
{
    public UpdateStaffRequestValidator()
    {
        RuleFor(x => x.StaffName)
            .NotEmpty()
            .WithMessage("Staff name is required")
            .MaximumLength(255)
            .WithMessage("Staff name cannot exceed 255 characters");

        RuleFor(x => x.Gender)
            .Must(gender => gender == null || gender == "Male" || gender == "Female" || gender == "Other")
            .WithMessage("Gender must be Male, Female, or Other");

        RuleFor(x => x.Dob)
            .LessThan(DateOnly.FromDateTime(DateTime.Today.AddYears(-18)))
            .WithMessage("Staff must be at least 18 years old")
            .When(x => x.Dob.HasValue);

        RuleFor(x => x.Dob)
            .GreaterThan(DateOnly.FromDateTime(DateTime.Today.AddYears(-65)))
            .WithMessage("Staff cannot be older than 65 years")
            .When(x => x.Dob.HasValue);

        RuleFor(x => x.Address)
            .MaximumLength(500)
            .WithMessage("Address cannot exceed 500 characters");
    }
}

public class UpdateStaffProfileRequestValidator : AbstractValidator<UpdateStaffProfileRequest>
{
    public UpdateStaffProfileRequestValidator()
    {
        RuleFor(x => x.StaffName)
            .NotEmpty()
            .WithMessage("Staff name is required")
            .MaximumLength(255)
            .WithMessage("Staff name cannot exceed 255 characters");

        RuleFor(x => x.Address)
            .MaximumLength(500)
            .WithMessage("Address cannot exceed 500 characters");
    }
}
