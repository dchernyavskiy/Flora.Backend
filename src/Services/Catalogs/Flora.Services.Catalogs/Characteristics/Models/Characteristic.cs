using BuildingBlocks.Core.Domain;
using Flora.Services.Catalogs.Categories;

namespace Flora.Services.Catalogs.Characteristics.Models;

public class Characteristic : Aggregate<Guid>
{
    public Characteristic()
    {
        Id = Guid.NewGuid();
    }

    public string Name { get; set; } = null!;

    public Guid CategoryId { get; set; }
    public Category Category { get; set; } = null!;

    public ICollection<CharacteristicValue> CharacteristicValues { get; set; } =
        null!;
}
