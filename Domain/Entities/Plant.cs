using System.ComponentModel.DataAnnotations;
using Flora.Domain.Common;

namespace Flora.Domain.Entities;

public class Plant : BaseEntity
{
    public string Name { get; set; } = null!;
    [DataType("decimal(8,2)")] public decimal Price { get; set; }
    public string Description { get; set; }
    
    public ICollection<Category> Categories { get; set; }
    public ICollection<Basket> Baskets { get; set; }
    public ICollection<CharacteristicPlant> CharacteristicPlants { get; set; }
}