using Flora.Application.Common.Interfaces;
using MediatR;

namespace Flora.Application.Baskets.Commands.ClearBasket;

public record ClearBasketCommand() : IRequest;

public class ClearBasketCommandHandler : IRequestHandler<ClearBasketCommand>
{
    private readonly IBasketService _basketService;

    public ClearBasketCommandHandler(IBasketService basketService)
    {
        _basketService = basketService;
    }

    public Task<Unit> Handle(ClearBasketCommand request, CancellationToken cancellationToken)
    {
        _basketService.Clear();
        return Task.FromResult(Unit.Value);
    }
}