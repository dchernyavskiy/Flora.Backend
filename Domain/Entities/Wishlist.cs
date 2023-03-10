using Flora.Domain.Common;

namespace Flora.Domain.Entities;

public class Wishlist : BaseEntity
{
    public Guid UserId { get; set; }
    public ICollection<Plant> Plants { get; set; }
}