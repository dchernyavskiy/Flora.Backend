using Flora.Services.Catalogs.Products.Models;

namespace Flora.Services.Catalogs.Products.Features.UpdatingProduct.v1;

public record UpdateProductRequest
{
    public string Name { get; init; } = null!;
    public decimal Price { get; init; }
    public int RestockThreshold { get; init; }
    public int MaxStockThreshold { get; init; }
    public ProductStatus Status { get; init; }
    public int Height { get; init; }
    public int Width { get; init; }
    public int Depth { get; init; }
    public string Size { get; init; } = null!;
    public Guid CategoryId { get; init; }
    public Guid BrandId { get; init; }
    public string? Description { get; init; }
}
