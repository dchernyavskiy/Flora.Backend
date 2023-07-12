using BuildingBlocks.Core.Domain;
using Flora.Services.Catalogs.Products.Models;

namespace Flora.Services.Catalogs.Characteristics.Models;

public class CharacteristicValue : Aggregate<Guid>
{
    public CharacteristicValue()
    {
        Id = Guid.NewGuid();
    }

    public string Value { get; set; } = null!;

    public Guid ProductId { get; set; }
    public Product Product { get; set; } = null!;
    public Guid CharacteristicId { get; set; }
    public Characteristic Characteristic { get; set; } = null!;
}
