using BuildingBlocks.Abstractions.Persistence;
using BuildingBlocks.Abstractions.Web.Module;
using Flora.Services.Catalogs.Characteristics.Data;
using Flora.Services.Catalogs.Shared;

namespace Flora.Services.Catalogs.Characteristics;

public class CharacteristicConfigs : IModuleConfiguration
{
    public const string Tag = "Characteristics";

    public const string CharacteristicPrefixUri =
        $"{SharedModulesConfiguration.CatalogModulePrefixUri}/characteristics";

    public WebApplicationBuilder AddModuleServices(WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<IDataSeeder, CharacteristicDataSeeder>();
        builder.Services.AddScoped<IDataSeeder, CharacteristicValueDataSeeder>();
        return builder;
    }

    public async Task<WebApplication> ConfigureModule(WebApplication app)
    {
        return app;
    }

    public IEndpointRouteBuilder MapEndpoints(IEndpointRouteBuilder endpoints)
    {
        return endpoints;
    }
}

public class CharacteristicValueConfigs : IModuleConfiguration
{
    public const string Tag = "CharacteristicValues";

    public const string CharacteristiValuePrefixUri =
        $"{SharedModulesConfiguration.CatalogModulePrefixUri}/characteristic-values";

    public WebApplicationBuilder AddModuleServices(WebApplicationBuilder builder)
    {
        return builder;
    }

    public async Task<WebApplication> ConfigureModule(WebApplication app)
    {
        return app;
    }

    public IEndpointRouteBuilder MapEndpoints(IEndpointRouteBuilder endpoints)
    {
        return endpoints;
    }
}
