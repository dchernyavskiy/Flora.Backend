using Flora.Domain.Common;

namespace Flora.Domain.Entities;

public class Basket : BaseEntity
{
    public Guid UserId { get; set; }
    public ICollection<Plant> Plants { get; set; }
}
