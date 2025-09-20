using FluentValidation;
using VisionCare.Application.Queries;

namespace VisionCare.Application.Validators;

public class GetUserByIdQueryValidator : AbstractValidator<GetUserByIdQuery>
{
    public GetUserByIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("User ID must be greater than 0");
    }
}
