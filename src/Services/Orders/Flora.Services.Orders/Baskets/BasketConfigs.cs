using BuildingBlocks.Abstractions.Web.Module;
using Flora.Services.Orders.Shared;

namespace Flora.Services.Orders.Baskets;

internal class BasketConfigs : IModuleConfiguration
{
    public const string Tag = "Baskets";
    public const string BusketsPrefixUri = $"{SharedModulesConfiguration.OrderModulePrefixUri}/baskets";

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
