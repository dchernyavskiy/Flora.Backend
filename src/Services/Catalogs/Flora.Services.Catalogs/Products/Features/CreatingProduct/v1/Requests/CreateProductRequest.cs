using BuildingBlocks.Abstractions.Mapping;
using Flora.Services.Catalogs.Products.Models;

namespace Flora.Services.Catalogs.Products.Features.CreatingProduct.v1.Requests;

public record CreateProductRequest : IMapWith<CreateProduct>
{
    public string Name { get; init; } = null!;
    public decimal Price { get; init; }
    public int Stock { get; init; }
    public int RestockThreshold { get; init; }
    public int MaxStockThreshold { get; init; }
    public ProductStatus Status { get; init; } = ProductStatus.Available;
    public int Height { get; init; }
    public int Width { get; init; }
    public int Depth { get; init; }
    public string Size { get; init; } = null!;
    public long CategoryId { get; init; }
    public long BrandId { get; init; }
    public string? Description { get; init; }
    public IEnumerable<CreateProductImageRequest>? Images { get; init; }
}
