using BuildingBlocks.Abstractions.Web.Module;
using Flora.Services.Orders.Shared;

namespace Flora.Services.Orders.Orders;

internal class OrderConfigs : IModuleConfiguration
{
    public const string Tag = "Orders";
    public const string OrdersPrefixUri = $"{SharedModulesConfiguration.OrderModulePrefixUri}/orders";

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
