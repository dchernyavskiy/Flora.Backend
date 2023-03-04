using Flora.Domain.Common;
using MongoDB.Bson.Serialization.Attributes;

namespace Flora.Domain.Entities;

public class CharacteristicValue : BaseEntity
{
    public string Value { get; set; }

    [BsonIgnore]public Guid PlantId { get; set; }
    [BsonIgnore]public Plant Plant { get; set; }
    public Guid CharacteristicId { get; set; }
    [BsonIgnore]public Characteristic Characteristic { get; set; }
}