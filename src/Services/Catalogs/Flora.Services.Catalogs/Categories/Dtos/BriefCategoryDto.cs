using BuildingBlocks.Abstractions.Mapping;
using Flora.Services.Catalogs.Products.Dtos.v1;
using Flora.Services.Catalogs.Products.Models;

namespace Flora.Services.Catalogs.Categories.Dtos;

public class BriefCategoryDto : IMapWith<Category>
{
    public string Name { get; set; } = default!;
    public Image? Image { get; set; } = default!;
    ICollection<ProductDto> Products { get; set; }
}

public class CategoryDto : IMapWith<Category>
{
    public string Name { get; set; } = default!;
    public Image? Image { get; set; } = default!;
    public string Description { get; set; } = default!;
    ICollection<ProductDto> Products { get; set; }
}
