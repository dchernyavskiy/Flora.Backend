using Flora.Domain.Common;

namespace Flora.Domain.Entities;

public class FeatureValue : BaseEntity
{
    public string Value { get; set; } = null!;

    public Guid FeatureId { get; set; }
    public Feature Feature { get; set; } = null!;
    public Guid PlantId { get; set; }
    public Plant Plant { get; set; } = null!;
}