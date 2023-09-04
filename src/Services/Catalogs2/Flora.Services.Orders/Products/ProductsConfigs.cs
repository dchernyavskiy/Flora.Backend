using BuildingBlocks.Abstractions.CQRS.Events;
using BuildingBlocks.Abstractions.Persistence;
using BuildingBlocks.Abstractions.Web.Module;
using Flora.Services.Orders.Shared;

namespace Flora.Services.Orders.Products;

internal class ProductsConfigs : IModuleConfiguration
{
    public const string Tag = "Products";
    public const string ProductsPrefixUri = $"{SharedModulesConfiguration.OrderModulePrefixUri}/products";

    public WebApplicationBuilder AddModuleServices(WebApplicationBuilder builder)
    {
        return builder;
    }

    public Task<WebApplication> ConfigureModule(WebApplication app)
    {
        return Task.FromResult(app);
    }

    public IEndpointRouteBuilder MapEndpoints(IEndpointRouteBuilder endpoints)
    {
        return endpoints;
    }
}
