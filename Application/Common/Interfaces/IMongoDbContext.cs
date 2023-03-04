using Flora.Domain.Entities;
using MongoDB.Driver;

namespace Flora.Application.Common.Interfaces;

public interface IMongoDbContext
{
    IMongoDatabase Database { get; }
    IMongoCollection<Category> Categories { get; }
    IMongoCollection<Plant> Plants { get; }
    IMongoCollection<Order> Orders { get; }
    IMongoCollection<Wishlist> Wishlists { get; }
}