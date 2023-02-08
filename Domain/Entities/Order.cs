using Flora.Domain.Common;

namespace Flora.Domain.Entities;

public class Order : BaseEntity
{
    public Guid UserId { get; set; }
    public DateTime OrderDate { get; set; }
    public ICollection<OrderItem> OrderItems { get; set; }
}