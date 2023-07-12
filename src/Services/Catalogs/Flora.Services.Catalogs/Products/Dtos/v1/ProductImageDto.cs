using BuildingBlocks.Abstractions.Mapping;
using Flora.Services.Catalogs.Products.Models;

namespace Flora.Services.Catalogs.Products.Dtos.v1;

public record ProductImageDto : IMapWith<Image>
{
    public long Id { get; init; }
    public string ImageUrl { get; init; } = default!;
    public bool IsMain { get; init; }
    public long ProductId { get; init; }
}
