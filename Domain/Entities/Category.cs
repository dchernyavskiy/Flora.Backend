using System.ComponentModel.DataAnnotations;
using Flora.Domain.Common;
using Flora.Domain.ValueObjects;

namespace Flora.Domain.Entities;

public class Category : BaseEntity
{
    [StringLength(200)]public string Name { get; set; }
    public Guid? ParentId { get; set; }
    public Category? Parent { get; set; }
    public Photo? Photo { get; set; }

    public ICollection<Category> Children { get; set; }
    public ICollection<Plant> Plants { get; set; }
    public ICollection<Characteristic> Characteristics { get; set; }
}