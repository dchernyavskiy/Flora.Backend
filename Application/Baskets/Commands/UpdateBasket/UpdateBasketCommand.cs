using Flora.Application.Baskets.Queries.GetBasket;
using Flora.Application.Common.Interfaces;
using Flora.Application.Plants.Common;
using MediatR;

namespace Flora.Application.Baskets.Commands.UpdateBasket;

public record UpdateBasketCommand : IRequest
{
    public ICollection<BasketItemDto> Items { get; set; }
}

public class UpdateBasketCommandHandler : IRequestHandler<UpdateBasketCommand>
{
    private readonly IBasketService _basketService;


    public UpdateBasketCommandHandler(IBasketService basketService)
    {
        _basketService = basketService;
    }

    public Task<Unit> Handle(UpdateBasketCommand request, CancellationToken cancellationToken)
    {
        var items = request.Items.Select(x => new BasketItemBriefDto()
        {
            PlantId = x.Plant.Id,
            Quantity = x.Quantity
        });
        _basketService.Clear();
        _basketService.AddBasketItems(items);
        return Task.FromResult(Unit.Value);
    }
}