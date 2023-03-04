using System.Reactive.Joins;
using Flora.Application.Common.Interfaces;
using Flora.Domain.Entities;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace Flora.Infrastructure.Persistence;

public class MongoDbContext : IMongoDbContext
{
    private readonly MongoClient _mongoClient;
    public IMongoDatabase Database { get; }
    public IMongoCollection<Category> Categories => Database.GetCollection<Category>(nameof(Categories));
    public IMongoCollection<Plant> Plants => Database.GetCollection<Plant>(nameof(Plants));
    public IMongoCollection<Order> Orders => Database.GetCollection<Order>(nameof(Categories));
    public IMongoCollection<Wishlist> Wishlists => Database.GetCollection<Wishlist>(nameof(Categories));

    public MongoDbContext(MongoClient mongoClient, IConfiguration configuration)
    {
        _mongoClient = mongoClient;
        Database = mongoClient.GetDatabase(configuration["MongoDb:DbName"]);
    }
}