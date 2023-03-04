using System.ComponentModel.DataAnnotations;
using Flora.Domain.Common;
using MongoDB.Bson.Serialization.Attributes;

namespace Flora.Domain.Entities;

public class Review : BaseEntity
{
    [StringLength(1000)] public string Comment { get; set; }
    [StringLength(50)] public string FullName { get; set; }
    [EmailAddress][StringLength(100)] public string Email { get; set; }
    [Range(0, 5)] public int? Rate { get; set; }
    public DateTime PostDate { get; set; }

    [BsonIgnore]public Guid? ParentId { get; set; } = null;
    [BsonIgnore]public Review Parent { get; set; } = null;
    [BsonIgnoreIfNull]public ICollection<Review> Children { get; set; }

    [BsonIgnore]public Guid? PlantId { get; set; } = null;
    [BsonIgnore]public Plant? Plant { get; set; } = null;
}