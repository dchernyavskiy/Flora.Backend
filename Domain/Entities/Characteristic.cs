using Flora.Domain.Common;

namespace Flora.Domain.Entities;

public class Characteristic : BaseEntity
{
    public string Name { get; set; }
    public ICollection<Category> Categories { get; set; }
    public ICollection<CharacteristicPlant> CharacteristicPlants { get; set; }
}