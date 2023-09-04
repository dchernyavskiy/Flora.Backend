using Microsoft.EntityFrameworkCore;

namespace Flora.Services.Wishlists.Shared.Contracts;

public interface IWishlistDbContext
{
    DbSet<TEntity> Set<TEntity>()
    where TEntity : class;

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
