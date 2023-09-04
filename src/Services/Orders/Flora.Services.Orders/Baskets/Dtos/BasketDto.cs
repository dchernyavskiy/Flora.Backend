using BuildingBlocks.Abstractions.Mapping;
using Flora.Services.Orders.Baskets.Models;

namespace Flora.Services.Orders.Baskets.Dtos;

public class BasketDto : IMapWith<Basket>
{
    public ICollection<BasketItemDto> BasketItems { get; set; }
}

public class BasketItemDto : IMapWith<BasketItem>
{
    public int Quantity { get; set; }
    public Guid ProductId { get; set; }
}
