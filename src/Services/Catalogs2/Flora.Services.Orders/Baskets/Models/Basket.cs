using BuildingBlocks.Core.Domain;
using Flora.Services.Orders.Products.Models;

namespace Flora.Services.Orders.Baskets.Models;

public class Basket : Aggregate<Guid>
{
    public Basket()
    {
        Id = Guid.NewGuid();
    }

    public Guid CustomerId { get; set; }
    public ICollection<BasketItem> BasketItems { get; set; }
}

public class BasketItem : Aggregate<Guid>
{
    public int Quantity { get; set; }

    public Guid BasketId { get; set; }
    public Basket Basket { get; set; }
    public Guid ProductId { get; set; }
    public Product Product { get; set; }
}
