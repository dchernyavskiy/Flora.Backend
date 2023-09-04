using BuildingBlocks.Persistence.EfCore.Postgres;

namespace Flora.Services.Orders.Shared.Data;

public class CatalogDbContextDesignFactory : DbContextDesignFactoryBase<OrderDbContext>
{
    public CatalogDbContextDesignFactory()
        : base("PostgresOptions:ConnectionString") { }
}
