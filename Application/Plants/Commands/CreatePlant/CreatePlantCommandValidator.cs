using FluentValidation;

namespace Flora.Application.Plants.Commands.CreatePlant;

public class CreatePlantCommandValidator : AbstractValidator<CreatePlantCommand>
{
    public CreatePlantCommandValidator()
    {
        RuleFor(x => x.Description)
            .MaximumLength(1000)
            .NotEmpty();
        RuleFor(x => x.Name)
            .MaximumLength(100)
            .NotEmpty();
        RuleFor(x => x.Price)
            .GreaterThan(0m);
        RuleFor(x => x.CategoryIds)
            .Must(x => x.Count >= 1)
            .NotEmpty();
    }
}