using BuildingBlocks.Abstractions.Persistence;
using BuildingBlocks.Abstractions.Web.Module;
using Flora.Services.Catalogs.Categories.Data;
using Flora.Services.Catalogs.Shared;

namespace Flora.Services.Catalogs.Categories;

internal class CategoryConfigs : IModuleConfiguration
{
    public const string Tag = "Categories";
    public const string CategoriesPrefixUri = $"{SharedModulesConfiguration.CatalogModulePrefixUri}/categories";
    public WebApplicationBuilder AddModuleServices(WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<IDataSeeder, CategoryDataSeeder>();

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
