using Flora.Domain.Common;

namespace Flora.Domain.ValueObjects;

public class Photo : ValueObject
{
    public string Link { get; set; }
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Link;
    }
}