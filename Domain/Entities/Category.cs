using System.ComponentModel.DataAnnotations;
using Flora.Domain.Common;
using MongoDB.Bson.Serialization.Attributes;

namespace Flora.Domain.Entities;

public class Category : BaseEntity
{
    [StringLength(200)] public string Name { get; set; }
    [BsonIgnore] public Guid? ParentId { get; set; }
    [BsonIgnore] public Category Parent { get; set; }

    [BsonIgnoreIfNull] public ICollection<Category> Children { get; set; }
    [BsonIgnoreIfNull] public ICollection<Plant> Plants { get; set; }
    [BsonIgnoreIfNull] public ICollection<Characteristic> Characteristics { get; set; }
}