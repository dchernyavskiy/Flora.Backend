using Flora.Domain.Common;

namespace Flora.Domain.Entities;

public class BasketItem : BaseEntity
{
    public int Quantity { get; set; }
    
    public Guid BasketId { get; set; }
    public Guid PlantId { get; set; }
    public Plant Plant { get; set; }
    public Basket Basket { get; set; }
}