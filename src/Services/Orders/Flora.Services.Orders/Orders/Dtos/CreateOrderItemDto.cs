using BuildingBlocks.Abstractions.Mapping;
using Flora.Services.Orders.Orders.Models;
using Flora.Services.Orders.Products.Models;

namespace Flora.Services.Orders.Orders.Dtos;

public class CreateOrderItemDto : IMapWith<OrderItem>
{
    public int Quantity { get; set; }

    public Guid ProductId { get; set; }
}

public class OrderItemDto : IMapWith<OrderItem>
{
    public int Quantity { get; set; }

    public Guid ProductId { get; set; }
    public Product Product { get; set; }
}
