using BuildingBlocks.Abstractions.Mapping;
using Flora.Services.Orders.Orders.Models;

namespace Flora.Services.Orders.Orders.Dtos;

public class OrderDto : IMapWith<Order>
{
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Phone { get; set; } = null!;
    public string Email { get; set; } = null!;
    public ICollection<OrderItemDto> OrderItems { get; set; } = null!;
}
