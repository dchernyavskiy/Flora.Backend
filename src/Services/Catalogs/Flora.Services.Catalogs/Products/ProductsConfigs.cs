using BuildingBlocks.Abstractions.CQRS.Events;
using BuildingBlocks.Abstractions.Persistence;
using BuildingBlocks.Abstractions.Web.Module;
using Flora.Services.Catalogs.Products.Data;
using Flora.Services.Catalogs.Products.Features.CreatingProduct.v1;
using Flora.Services.Catalogs.Products.Features.DebitingProductStock.v1;
using Flora.Services.Catalogs.Products.Features.GettingProductById.v1;
using Flora.Services.Catalogs.Products.Features.ReplenishingProductStock.v1;
using Flora.Services.Catalogs.Products.Features.UpdatingProduct.v1;
using Flora.Services.Catalogs.Shared;

namespace Flora.Services.Catalogs.Products;

internal class ProductsConfigs : IModuleConfiguration
{
    public const string Tag = "Products";
    public const string ProductsPrefixUri = $"{SharedModulesConfiguration.CatalogModulePrefixUri}/products";

    public WebApplicationBuilder AddModuleServices(WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<IDataSeeder, ProductDataSeeder>();
        builder.Services.AddSingleton<IEventMapper, ProductEventMapper>();

        return builder;
    }

    public Task<WebApplication> ConfigureModule(WebApplication app)
    {
        return Task.FromResult(app);
    }

    public IEndpointRouteBuilder MapEndpoints(IEndpointRouteBuilder endpoints)
    {
        var productsVersionGroup = endpoints.MapApiGroup(Tag).WithTags(Tag);

        var productsGroupV1 = productsVersionGroup.MapGroup(ProductsPrefixUri).HasApiVersion(1.0);

        var productsGroupV2 = productsVersionGroup.MapGroup(ProductsPrefixUri).HasApiVersion(2.0);

        productsGroupV1.MapCreateProductsEndpoint();
        productsGroupV1.MapUpdateProductEndpoint();
        productsGroupV1.MapDebitProductStockEndpoint();
        productsGroupV1.MapReplenishProductStockEndpoint();
        productsGroupV1.MapGetProductByIdEndpoint();

        return endpoints;
    }
}
