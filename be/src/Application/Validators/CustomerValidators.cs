using FluentValidation;
using VisionCare.Application.DTOs.CustomerDto;

namespace VisionCare.Application.Validators;

public class CreateCustomerRequestValidator : AbstractValidator<CreateCustomerRequest>
{
    public CreateCustomerRequestValidator()
    {
        RuleFor(x => x.AccountId)
            .GreaterThan(0)
            .WithMessage("Account ID must be greater than 0");

        RuleFor(x => x.CustomerName)
            .NotEmpty()
            .WithMessage("Customer name is required")
            .MaximumLength(255)
            .WithMessage("Customer name cannot exceed 255 characters");

        RuleFor(x => x.Gender)
            .Must(gender => gender == null || gender == "Male" || gender == "Female" || gender == "Other")
            .WithMessage("Gender must be Male, Female, or Other");

        RuleFor(x => x.Dob)
            .LessThan(DateOnly.FromDateTime(DateTime.Today))
            .WithMessage("Date of birth must be in the past")
            .When(x => x.Dob.HasValue);

        RuleFor(x => x.Dob)
            .GreaterThan(DateOnly.FromDateTime(DateTime.Today.AddYears(-120)))
            .WithMessage("Date of birth cannot be more than 120 years ago")
            .When(x => x.Dob.HasValue);

        RuleFor(x => x.Address)
            .MaximumLength(500)
            .WithMessage("Address cannot exceed 500 characters");
    }
}

public class UpdateCustomerRequestValidator : AbstractValidator<UpdateCustomerRequest>
{
    public UpdateCustomerRequestValidator()
    {
        RuleFor(x => x.CustomerName)
            .NotEmpty()
            .WithMessage("Customer name is required")
            .MaximumLength(255)
            .WithMessage("Customer name cannot exceed 255 characters");

        RuleFor(x => x.Gender)
            .Must(gender => gender == null || gender == "Male" || gender == "Female" || gender == "Other")
            .WithMessage("Gender must be Male, Female, or Other");

        RuleFor(x => x.Dob)
            .LessThan(DateOnly.FromDateTime(DateTime.Today))
            .WithMessage("Date of birth must be in the past")
            .When(x => x.Dob.HasValue);

        RuleFor(x => x.Dob)
            .GreaterThan(DateOnly.FromDateTime(DateTime.Today.AddYears(-120)))
            .WithMessage("Date of birth cannot be more than 120 years ago")
            .When(x => x.Dob.HasValue);

        RuleFor(x => x.Address)
            .MaximumLength(500)
            .WithMessage("Address cannot exceed 500 characters");
    }
}

public class UpdateCustomerProfileRequestValidator : AbstractValidator<UpdateCustomerProfileRequest>
{
    public UpdateCustomerProfileRequestValidator()
    {
        RuleFor(x => x.CustomerName)
            .NotEmpty()
            .WithMessage("Customer name is required")
            .MaximumLength(255)
            .WithMessage("Customer name cannot exceed 255 characters");

        RuleFor(x => x.Address)
            .MaximumLength(500)
            .WithMessage("Address cannot exceed 500 characters");
    }
}
