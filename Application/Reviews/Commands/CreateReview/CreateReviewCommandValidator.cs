using FluentValidation;

namespace Flora.Application.Reviews.Commands.CreateReview;

public class CreateReviewCommandValidator : AbstractValidator<CreateReviewCommand>
{
    public CreateReviewCommandValidator()
    {
        RuleFor(x => x.Rate)
            .LessThanOrEqualTo(5)
            .GreaterThanOrEqualTo(0);
            
        RuleFor(x => x.Message)
            .MinimumLength(1)
            .MaximumLength(300);
    }
}