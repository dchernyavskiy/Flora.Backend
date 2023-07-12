using BuildingBlocks.Persistence.EfCore.Postgres;

namespace Flora.Services.Identity.Shared.Data;

public class DbContextDesignFactory : DbContextDesignFactoryBase<IdentityContext>
{
    public DbContextDesignFactory()
        : base("PostgresOptions:ConnectionString") { }
}
