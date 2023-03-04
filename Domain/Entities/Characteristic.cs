using System.ComponentModel.DataAnnotations;
using Flora.Domain.Common;
using MongoDB.Bson.Serialization.Attributes;

namespace Flora.Domain.Entities;

public class Characteristic : BaseEntity
{
    [StringLength(1000)] public string Name { get; set; }
    [BsonIgnore]public Guid CategoryId { get; set; }
    [BsonIgnore]public Category Category { get; set; }

    [BsonIgnore]public ICollection<CharacteristicValue> CharacteristicValues { get; set; }
}