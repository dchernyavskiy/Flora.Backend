using BuildingBlocks.Core.Domain;
using Flora.Services.Orders.Products.Models;

namespace Flora.Services.Orders.Orders.Models;

public class OrderItem : Aggregate<Guid>
{
    public OrderItem()
    {
        Id = Guid.NewGuid();
    }

    public int Quantity { get; set; }

    public Guid ProductId { get; set; }
    public Product Product { get; set; }
    public Guid OrderId { get; set; }
    public Order Order { get; set; }
}
