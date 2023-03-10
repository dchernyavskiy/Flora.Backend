using Flora.Application.Common.Interfaces;
using MediatR;

namespace Flora.Application.Baskets.Queries.GetBasketCount;

public class BasketCount
{
    public int Count { get; set; }
}

public record GetBasketCountQuery() : IRequest<BasketCount>;

public class GetBasketCountQueryHandler : IRequestHandler<GetBasketCountQuery, BasketCount>
{
    private readonly IBasketService _basketService;

    public GetBasketCountQueryHandler(IBasketService basketService)
    {
        _basketService = basketService;
    }
    
    public Task<BasketCount> Handle(GetBasketCountQuery request, CancellationToken cancellationToken)
    {
        return Task.FromResult(new BasketCount()
        {
            Count = _basketService.GetBasketItems().Count
        });
    }
}