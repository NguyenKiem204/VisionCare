using FluentValidation;
using VisionCare.Application.DTOs.BlogDto;

namespace VisionCare.Application.Validators.BlogDto;

public class UpdateBlogRequestValidator : AbstractValidator<UpdateBlogRequest>
{
    public UpdateBlogRequestValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty()
            .WithMessage("Title is required")
            .MaximumLength(500)
            .WithMessage("Title cannot exceed 500 characters");

        RuleFor(x => x.Content)
            .NotEmpty()
            .WithMessage("Content is required");

        RuleFor(x => x.Slug)
            .MaximumLength(500)
            .WithMessage("Slug cannot exceed 500 characters")
            .Matches("^[a-z0-9]+(?:-[a-z0-9]+)*$")
            .WithMessage("Slug must be URL-friendly (lowercase letters, numbers, and hyphens only)")
            .When(x => !string.IsNullOrWhiteSpace(x.Slug));

        RuleFor(x => x.Excerpt)
            .MaximumLength(1000)
            .WithMessage("Excerpt cannot exceed 1000 characters")
            .When(x => !string.IsNullOrWhiteSpace(x.Excerpt));

        RuleFor(x => x.Status)
            .Must(status => status == null || new[] { "Draft", "Published", "Archived" }.Contains(status))
            .WithMessage("Status must be Draft, Published, or Archived")
            .When(x => !string.IsNullOrWhiteSpace(x.Status));
    }
}

