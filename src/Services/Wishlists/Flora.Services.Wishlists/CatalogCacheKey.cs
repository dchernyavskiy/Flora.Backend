namespace Flora.Services.Wishlists;

public static class CatalogCacheKey
{
    public static string ProductsByCategory(long categoryId) => $"{nameof(ProductsByCategory)}{categoryId}";

    public static string ProductsWithDiscounts(long id) => $"{nameof(ProductsWithDiscounts)}{id}";
}
