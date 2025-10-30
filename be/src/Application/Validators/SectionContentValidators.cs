using FluentValidation;
using VisionCare.Application.DTOs.SectionContentDto;

namespace VisionCare.Application.Validators;

public class WhyUsUpsertValidator : AbstractValidator<WhyUsUpsertDto>
{
    public WhyUsUpsertValidator()
    {
        RuleFor(x => x.Title).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Subtitle).NotEmpty().MaximumLength(1000);
        RuleFor(x => x.Bullets)
            .NotNull()
            .Must(b => b.Count == 4)
            .WithMessage("Bullets phải có đúng 4 mục");
        RuleFor(x => x.Images)
            .Must(b => b == null || b.Count == 0 || b.Count == 4)
            .WithMessage("Images phải để trống hoặc đủ 4 ảnh");
    }
}

public class AboutUpsertValidator : AbstractValidator<AboutUpsertDto>
{
    public AboutUpsertValidator()
    {
        RuleFor(x => x.Title).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Content).NotEmpty();
    }
}
