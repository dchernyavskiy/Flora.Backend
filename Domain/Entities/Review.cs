using System.ComponentModel.DataAnnotations;
using Flora.Domain.Common;

namespace Flora.Domain.Entities;

public class Review : BaseEntity
{
    [StringLength(300)] public string Message { get; set; }
    public Guid UserId { get; set; }
    public Guid ParentId { get; set; }
    public Review Parent { get; set; }
    
    public Guid PlantId { get; set; }
    public Plant Plant { get; set; }
}