using BuildingBlocks.Abstractions.CQRS.Events.Internal;
using BuildingBlocks.Core.Persistence.EfCore;
using Flora.Services.Orders.Baskets.Models;
using Flora.Services.Orders.Orders.Models;
using Flora.Services.Orders.Products.Models;
using Flora.Services.Orders.Shared.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Flora.Services.Orders.Shared.Data;

public class OrderDbContext : EfDbContextBase, IOrderDbContext
{
    public const string DefaultSchema = "order";

    public OrderDbContext(DbContextOptions<OrderDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        base.OnModelCreating(modelBuilder);
    }

    public DbSet<Order> Orders => Set<Order>();
    public DbSet<OrderItem> OrderItems => Set<OrderItem>();
    public DbSet<Basket> Baskets => Set<Basket>();
    public DbSet<BasketItem> BasketItems => Set<BasketItem>();
    public DbSet<Product> Products => Set<Product>();
}
