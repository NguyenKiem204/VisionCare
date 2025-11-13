using FluentValidation;
using VisionCare.Application.DTOs.BlogDto;

namespace VisionCare.Application.Validators.BlogDto;

public class CreateCommentRequestValidator : AbstractValidator<CreateCommentRequest>
{
    public CreateCommentRequestValidator()
    {
        // BlogId will be set from route parameter in controller, so skip validation here
        // The controller will validate blogId from route

        RuleFor(x => x.CommentText)
            .NotEmpty()
            .WithMessage("Comment text is required")
            .MaximumLength(2000)
            .WithMessage("Comment text cannot exceed 2000 characters");
    }
}

