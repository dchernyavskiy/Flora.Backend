using System.ComponentModel.DataAnnotations;
using Flora.Domain.Common;
using MongoDB.Bson.Serialization.Attributes;

namespace Flora.Domain.Entities;

public class Plant : BaseEntity
{
    [StringLength(100)] public string Name { get; set; } = null!;
    [DataType("decimal(8,2)")] public decimal Price { get; set; }
    [StringLength(1000)] public string Description { get; set; }
    [Range(0, 5)] public double? Rate { get; set; }
    public DateTime DeliveryDate { get; set; }

    [BsonIgnore]public Guid CategoryId { get; set; }
    [BsonIgnore]public Category Category { get; set; }
    [BsonIgnore]public ICollection<BasketItem> BasketItems { get; set; }
    [BsonIgnore]public ICollection<OrderItem> OrderItems { get; set; }
    [BsonIgnoreIfNull]public ICollection<CharacteristicValue> CharacteristicValues { get; set; }
    [BsonIgnoreIfNull]public ICollection<Review> Reviews { get; set; }
    [BsonIgnoreIfNull]public ICollection<Wishlist> Wishlists { get; set; }
}