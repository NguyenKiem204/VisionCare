using FluentValidation;
using VisionCare.Application.DTOs.ServiceDetailDto;

namespace VisionCare.Application.Validators;

public class CreateServiceDetailRequestValidator : AbstractValidator<CreateServiceDetailRequest>
{
    public CreateServiceDetailRequestValidator()
    {
        RuleFor(x => x.ServiceId).GreaterThan(0).WithMessage("Service ID must be greater than 0");

        RuleFor(x => x.ServiceTypeId)
            .GreaterThan(0)
            .WithMessage("Service Type ID must be greater than 0");

        RuleFor(x => x.Cost)
            .GreaterThan(0)
            .WithMessage("Cost must be greater than 0")
            .LessThan(1000000)
            .WithMessage("Cost cannot exceed 1,000,000");
    }
}

public class UpdateServiceDetailRequestValidator : AbstractValidator<UpdateServiceDetailRequest>
{
    public UpdateServiceDetailRequestValidator()
    {
        RuleFor(x => x.Cost)
            .GreaterThan(0)
            .WithMessage("Cost must be greater than 0")
            .LessThan(1000000)
            .WithMessage("Cost cannot exceed 1,000,000");
    }
}
