using Flora.Domain.Common;

namespace Flora.Domain.Entities;

public class Feature : BaseEntity
{
    public string Name { get; set; } = null!;

    public ICollection<FeatureValue> FeatureValues { get; set; } = null!;
}