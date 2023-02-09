using System.ComponentModel.DataAnnotations;
using Flora.Domain.Common;

namespace Flora.Domain.Entities;

public class Plant : BaseEntity
{
    [StringLength(100)] public string Name { get; set; } = null!;
    [DataType("decimal(8,2)")] public decimal Price { get; set; }
    [StringLength(1000)] public string Description { get; set; }

    public Guid CategoryId { get; set; }
    public Category Category { get; set; }
    public ICollection<BasketItem> BasketItems { get; set; }
    public ICollection<OrderItem> OrderItems { get; set; }
    public ICollection<CharacteristicValue> CharacteristicValues { get; set; }
    public ICollection<Review> Reviews { get; set; }
    public ICollection<Wishlist> Wishlists { get; set; }
}