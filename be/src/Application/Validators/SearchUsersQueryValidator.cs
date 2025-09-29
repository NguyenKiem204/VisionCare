using FluentValidation;
using VisionCare.Application.Queries.Users;

namespace VisionCare.Application.Validators;

public class SearchUsersQueryValidator : AbstractValidator<SearchUsersQuery>
{
    public SearchUsersQueryValidator()
    {
        RuleFor(x => x.Page).GreaterThan(0);
        RuleFor(x => x.PageSize).GreaterThan(0).LessThanOrEqualTo(100);
        RuleFor(x => x.Status)
            .Must(s =>
                string.IsNullOrWhiteSpace(s)
                || new[] { "Active", "Suspended", "Deleted" }.Contains(s)
            )
            .WithMessage("Status must be Active, Suspended, or Deleted");
    }
}
