namespace Flora.Services.Orders;

public static class OrderCacheKey
{
    public static string ProductsByCategory(long categoryId) => $"{nameof(ProductsByCategory)}{categoryId}";

    public static string ProductsWithDiscounts(long id) => $"{nameof(ProductsWithDiscounts)}{id}";
}
