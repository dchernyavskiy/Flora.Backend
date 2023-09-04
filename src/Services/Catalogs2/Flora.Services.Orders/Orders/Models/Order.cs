using BuildingBlocks.Core.Domain;

namespace Flora.Services.Orders.Orders.Models;

public class Order : Aggregate<Guid>
{
    public Order()
    {
        Id = Guid.NewGuid();
    }

    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Phone { get; set; } = null!;
    public string Email { get; set; } = null!;
    public Guid CustomerId { get; set; }
    public ICollection<OrderItem> OrderItems { get; set; } = null!;
}
