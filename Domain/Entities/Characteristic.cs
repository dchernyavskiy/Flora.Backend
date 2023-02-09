using System.ComponentModel.DataAnnotations;
using Flora.Domain.Common;

namespace Flora.Domain.Entities;

public class Characteristic : BaseEntity
{
    [StringLength(50)] public string Name { get; set; }
    public Guid CategoryId { get; set; }
    public Category Category { get; set; }

    public ICollection<CharacteristicValue> CharacteristicPlants { get; set; }
}