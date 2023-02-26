using AutoMapper;
using Flora.Application.Common.Interfaces;
using Flora.Application.Common.Mappings;
using Flora.Application.Plants.Commands.AddToBasket;
using Flora.Application.Plants.Common;
using Flora.Domain.Entities;
using MediatR;

namespace Flora.Application.Plants.Commands.RemoveFromBasket;

public record RemoveFromBasketCommand : IRequest<bool>, IMapWith<BasketItem>
{
    public Guid PlantId { get; set; }
};

public class RemoveFromBasketHandler : IRequestHandler<RemoveFromBasketCommand, bool>
{
    private readonly IBasketService _basketService;


    public RemoveFromBasketHandler(IBasketService basketService)
    {
        _basketService = basketService;
    }

    public Task<bool> Handle(RemoveFromBasketCommand request, CancellationToken cancellationToken)
    {
        _basketService.RemoveBasketItem(new BasketItemBriefDto() { PlantId = request.PlantId });
        return Task.FromResult(true);
    }
}