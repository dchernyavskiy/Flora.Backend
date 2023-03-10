using FluentValidation;

namespace Flora.Application.Reviews.Commands.CreateReview;

public class CreateReviewCommandValidator : AbstractValidator<CreateReviewCommand>
{
    public CreateReviewCommandValidator()
    {
        RuleFor(x => x.Rate)
            .LessThanOrEqualTo(5)
            .GreaterThanOrEqualTo(0);
            
        RuleFor(x => x.Comment)
            .MaximumLength(300)
            .NotEmpty()
            .NotNull();

        RuleFor(x => x.FullName)
            .MinimumLength(2)
            .NotNull()
            .NotEmpty();

        RuleFor(x => x.Email)
            .EmailAddress()
            .NotEmpty()
            .NotNull();

        RuleFor(x => x.PlantId)
            .NotEqual(Guid.Empty);
    }
}