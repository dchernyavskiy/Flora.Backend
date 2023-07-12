using BuildingBlocks.Core.CQRS.Queries;
using Flora.Services.Catalogs.Products.Dtos;
using Flora.Services.Catalogs.Products.Dtos.v1;

namespace Flora.Services.Catalogs.Products.Features.GettingProducts.v1;

public record GetProductsResponse(ListResultModel<ProductDto> Products);
