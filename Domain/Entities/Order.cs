using System.ComponentModel.DataAnnotations;
using Flora.Domain.Common;

namespace Flora.Domain.Entities;

public class Order : BaseEntity
{
    [StringLength(20)]public string FirstName { get; set; }
    [StringLength(20)]public string LastName { get; set; }
    [StringLength(15)]public string Phone { get; set; }
    [StringLength(50)][EmailAddress]public string Email { get; set; }
    public Guid UserId { get; set; }
    public DateTime OrderDate { get; set; }
    public ICollection<OrderItem> OrderItems { get; set; }
}