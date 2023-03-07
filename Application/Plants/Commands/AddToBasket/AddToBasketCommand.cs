using Flora.Application.Common.Interfaces;
using Flora.Application.Common.Mappings;
using Flora.Application.Plants.Common;
using Flora.Domain.Entities;
using MediatR;

namespace Flora.Application.Plants.Commands.AddToBasket;

public record AddToBasketCommand : BasketItemBriefDto, IRequest<bool>, IMapWith<BasketItem>;

public class AddToBasketCommandHandler : IRequestHandler<AddToBasketCommand, bool>
{
    private readonly IBasketService _basketService;
    
    public AddToBasketCommandHandler(IBasketService basketService)
    {
        _basketService = basketService;
    }

    public Task<bool> Handle(AddToBasketCommand request, CancellationToken cancellationToken)
    {
        _basketService.AddBasketItem(request);
        return Task.FromResult(true);
    }
}