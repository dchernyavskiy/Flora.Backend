using FluentValidation;

namespace Flora.Application.Plants.Commands.UpdatePlant;

public class UpdatePlantCommandValidator : AbstractValidator<UpdatePlantCommand>
{
    public UpdatePlantCommandValidator()
    {
        RuleFor(x => x.Description)
            .MaximumLength(1000)
            .NotEmpty();
        RuleFor(x => x.Name)
            .MaximumLength(100)
            .NotEmpty();
        RuleFor(x => x.Price)
            .GreaterThan(0m);
    }
}