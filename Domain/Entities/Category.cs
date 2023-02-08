using Flora.Domain.Common;

namespace Flora.Domain.Entities;

public class Category : BaseEntity
{
    public string Name { get; set; }
    public Guid ParentId { get; set; }
    public Category Parent { get; set; }

    public ICollection<Plant> Plants { get; set; }
}