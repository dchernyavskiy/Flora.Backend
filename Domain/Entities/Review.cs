using System.ComponentModel.DataAnnotations;
using Flora.Domain.Common;

namespace Flora.Domain.Entities;

public class Review : BaseEntity
{
    [StringLength(300)] public string Message { get; set; }
    [Required] public Guid UserId { get; set; }
    [StringLength(20)] public string UserFirstName { get; set; }
    [StringLength(20)] public string UserLastName { get; set; }

    [Range(0, 5)] public int Rate { get; set; }
    public DateTime PostDate { get; set; }

    public Guid? ParentId { get; set; } = null;
    public Review Parent { get; set; }

    public Guid PlantId { get; set; }
    public Plant Plant { get; set; }
}