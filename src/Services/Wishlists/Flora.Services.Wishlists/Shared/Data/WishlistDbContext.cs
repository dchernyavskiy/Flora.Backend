using BuildingBlocks.Abstractions.CQRS.Events.Internal;
using BuildingBlocks.Core.Persistence.EfCore;
using Flora.Services.Wishlists.Shared.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Flora.Services.Wishlists.Shared.Data;

public class WishlistDbContext : EfDbContextBase, IWishlistDbContext
{
    public const string DefaultSchema = "catalog";

    public WishlistDbContext(DbContextOptions<WishlistDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        base.OnModelCreating(modelBuilder);
    }
}
