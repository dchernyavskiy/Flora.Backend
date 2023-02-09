using System.Reactive.Joins;
using Flora.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Flora.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    public DbSet<Basket> Baskets { get; }
    public DbSet<BasketItem> BasketItems { get; }
    public DbSet<Category> Categories { get; }
    public DbSet<Order> Orders { get; }
    public DbSet<OrderItem> OrderItems { get; }
    public DbSet<Plant> Plants { get; }
    public DbSet<Wishlist> Wishlists { get; }
    public DbSet<Characteristic> Characteristics { get; }
    public DbSet<CharacteristicValue> CharacteristicValues { get; }
    public DbSet<Review> Reviews { get; }
    

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}