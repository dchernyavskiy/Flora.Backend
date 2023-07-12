using BuildingBlocks.Persistence.Mongo;
using Flora.Services.Catalogs.Products.Models.Read;
using Humanizer;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Flora.Services.Catalogs.Shared.Data;

public class CatalogReadDbContext : MongoDbContext
{
    public CatalogReadDbContext(IOptions<MongoOptions> options)
        : base(options.Value)
    {
        Products = GetCollection<ProductReadModel>(nameof(Products).Underscore());
    }

    public IMongoCollection<ProductReadModel> Products { get; }
}
