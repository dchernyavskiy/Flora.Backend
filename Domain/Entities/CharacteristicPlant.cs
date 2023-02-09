using Flora.Domain.Common;

namespace Flora.Domain.Entities;

public class CharacteristicPlant : BaseEntity
{
    public string Value { get; set; }

    public Guid PlantId { get; set; }
    public Plant Plant { get; set; }
    public Guid CharacteristicId { get; set; }
    public Characteristic Characteristic { get; set; }
}