using FluentValidation;
using VisionCare.Application.Commands.Users;

namespace VisionCare.Application.Validators;

public class DeleteUserCommandValidator : AbstractValidator<DeleteUserCommand>
{
    public DeleteUserCommandValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0).WithMessage("User ID must be greater than 0");
    }
}
