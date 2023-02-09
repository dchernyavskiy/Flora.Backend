using Flora.Domain.Common;

namespace Flora.Domain.Entities;

public class OrderItem : BaseEntity
{
    public int Quantity { get; set; }
    public Guid PlantId { get; set; }
    public Plant Plant { get; set; }

    public Guid OrderId { get; set; }
    public Order Order { get; set; }
}