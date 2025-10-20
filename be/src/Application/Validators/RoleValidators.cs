using FluentValidation;
using VisionCare.Application.DTOs.RoleDto;

namespace VisionCare.Application.Validators;

public class CreateRoleRequestValidator : AbstractValidator<CreateRoleRequest>
{
    public CreateRoleRequestValidator()
    {
        RuleFor(x => x.RoleName)
            .NotEmpty()
            .WithMessage("Role name is required")
            .MaximumLength(50)
            .WithMessage("Role name cannot exceed 50 characters")
            .Matches(@"^[a-zA-Z\s]+$")
            .WithMessage("Role name can only contain letters and spaces");
    }
}

public class UpdateRoleRequestValidator : AbstractValidator<UpdateRoleRequest>
{
    public UpdateRoleRequestValidator()
    {
        RuleFor(x => x.RoleName)
            .NotEmpty()
            .WithMessage("Role name is required")
            .MaximumLength(50)
            .WithMessage("Role name cannot exceed 50 characters")
            .Matches(@"^[a-zA-Z\s]+$")
            .WithMessage("Role name can only contain letters and spaces");
    }
}
