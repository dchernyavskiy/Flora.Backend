using BuildingBlocks.Abstractions.Mapping;
using Flora.Services.Catalogs.Products.Models;

namespace Flora.Services.Catalogs.Products.Dtos.v1;

public record ProductDto : IMapWith<Product>
{
    public long Id { get; init; }
    public string Name { get; init; } = default!;
    public string? Description { get; init; }
    public decimal Price { get; init; }
    public long CategoryId { get; init; }
    public string CategoryName { get; init; } = default!;
    public string SupplierName { get; init; } = default!;
    public long BrandId { get; init; }
    public string BrandName { get; init; } = default!;
    public int AvailableStock { get; init; }
    public int RestockThreshold { get; init; }
    public int MaxStockThreshold { get; init; }
    public ProductStatus ProductStatus { get; init; }
    public string Size { get; init; }
    public int Height { get; init; }
    public int Width { get; init; }
    public int Depth { get; init; }
    public IEnumerable<ProductImageDto>? Images { get; init; }
}
