using FluentValidation;

namespace Flora.Application.Characteristics.Commands.CreateCharacteristic;

public class CreateCharacteristicCommandValidator : AbstractValidator<CreateCharacteristicCommand>
{
    public CreateCharacteristicCommandValidator()
    {
        RuleFor(x => x.Name)
            .MinimumLength(2)
            .MaximumLength(50);
    }   
}