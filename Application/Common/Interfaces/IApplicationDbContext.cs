using System.Reactive.Joins;
using Flora.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Flora.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<Basket> Baskets { get; }
    DbSet<BasketItem> BasketItems { get; }
    DbSet<Category> Categories { get; }
    DbSet<Order> Orders { get; }
    DbSet<OrderItem> OrderItems { get; }
    DbSet<Plant> Plants { get; }
    DbSet<Wishlist> Wishlists { get; }
    DbSet<Characteristic> Characteristics { get; }
    DbSet<CharacteristicValue> CharacteristicValues { get; }
    DbSet<Review> Reviews { get; }
    

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    EntityEntry Entry(object entity);
}