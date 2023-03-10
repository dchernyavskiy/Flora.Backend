using FluentValidation;

namespace Flora.Application.Plants.Commands.AddToBasket;

public class AddToBasketCommandValidator : AbstractValidator<AddToBasketCommand>
{
    public AddToBasketCommandValidator() { }
}