using BuildingBlocks.Persistence.EfCore.Postgres;

namespace Flora.Services.Wishlists.Shared.Data;

public class CatalogDbContextDesignFactory : DbContextDesignFactoryBase<WishlistDbContext>
{
    public CatalogDbContextDesignFactory()
        : base("PostgresOptions:ConnectionString") { }
}
