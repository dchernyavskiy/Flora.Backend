using Flora.Services.Catalogs.Categories;
using Flora.Services.Catalogs.Characteristics.Models;
using Flora.Services.Catalogs.Products.Models;
using Microsoft.EntityFrameworkCore;

namespace Flora.Services.Catalogs.Shared.Contracts;

public interface ICatalogDbContext
{
    DbSet<Product> Products { get; }
    DbSet<Category> Categories { get; }
    DbSet<Characteristic> Characteristics { get; }
    DbSet<CharacteristicValue> CharacteristicValues { get; }

    DbSet<TEntity> Set<TEntity>()
    where TEntity : class;

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
