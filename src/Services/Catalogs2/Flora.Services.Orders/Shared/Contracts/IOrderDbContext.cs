using Flora.Services.Orders.Baskets.Models;
using Flora.Services.Orders.Orders.Models;
using Flora.Services.Orders.Products.Models;
using Microsoft.EntityFrameworkCore;

namespace Flora.Services.Orders.Shared.Contracts;

public interface IOrderDbContext
{
    DbSet<Order> Orders { get; }
    DbSet<OrderItem> OrderItems { get; }
    DbSet<Basket> Baskets { get; }
    DbSet<BasketItem> BasketItems { get; }
    DbSet<Product> Products { get; }


    DbSet<TEntity> Set<TEntity>()
    where TEntity : class;

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
