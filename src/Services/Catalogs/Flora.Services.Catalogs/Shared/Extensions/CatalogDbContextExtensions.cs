using Flora.Services.Catalogs.Categories;
using Flora.Services.Catalogs.Products.Models;
using Flora.Services.Catalogs.Shared.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Flora.Services.Catalogs.Shared.Extensions;

/// <summary>
/// Put some shared code between multiple feature here, for preventing duplicate some codes
/// Ref: https://www.youtube.com/watch?v=01lygxvbao4.
/// </summary>
public static class CatalogDbContextExtensions
{
    public static Task<bool> ProductExistsAsync(
        this ICatalogDbContext context,
        Guid id,
        CancellationToken cancellationToken = default
    )
    {
        return context.Products.AnyAsync(x => x.Id == id, cancellationToken: cancellationToken);
    }

    public static ValueTask<Product?> FindProductByIdAsync(this ICatalogDbContext context, Guid id)
    {
        return context.Products.FindAsync(id);
    }

    public static Task<bool> CategoryExistsAsync(
        this ICatalogDbContext context,
        Guid id,
        CancellationToken cancellationToken = default
    )
    {
        return context.Categories.AnyAsync(x => x.Id == id, cancellationToken: cancellationToken);
    }

    public static ValueTask<Category?> FindCategoryAsync(this ICatalogDbContext context, Guid id)
    {
        return context.Categories.FindAsync(id);
    }
}
