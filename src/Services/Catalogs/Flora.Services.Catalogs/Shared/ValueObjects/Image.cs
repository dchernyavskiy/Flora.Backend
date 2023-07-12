using BuildingBlocks.Core.Domain;

namespace Flora.Services.Catalogs.Products.Models;

public class Image : ValueObject
{
    public string ImageUrl { get; set; } = default!;
    public bool IsMain { get; set; }


    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return ImageUrl;
        yield return IsMain;
    }
}
