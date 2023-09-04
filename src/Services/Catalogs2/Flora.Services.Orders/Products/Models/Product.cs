using BuildingBlocks.Core.Domain;
using Flora.Services.Orders.Baskets.Models;
using Flora.Services.Orders.Orders.Models;

namespace Flora.Services.Orders.Products.Models;

public class Product : Aggregate<Guid>
{
    public Product()
    {
        Id = Guid.NewGuid();
    }

    public string ImageUrl { get; set; } = default!;
    public string Name { get; set; } = default!;
    public decimal Price { get; set; } = default!;
    public ProductStatus ProductStatus { get; set; }

    public ICollection<OrderItem> OrderItems { get; set; } = default!;
    public ICollection<BasketItem> BasketItems { get; set; } = default!;

    public static Product Create(
        Guid id,
        string imageUrl,
        string name,
        decimal price,
        ProductStatus status
    )
    {
        return new Product()
               {
                   Id = id,
                   ImageUrl = imageUrl,
                   Name = name,
                   Price = price,
                   ProductStatus = status
               };
    }
}
