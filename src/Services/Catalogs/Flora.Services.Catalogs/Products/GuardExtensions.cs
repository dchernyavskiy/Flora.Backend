using Ardalis.GuardClauses;
using Flora.Services.Catalogs.Products.Exceptions.Application;

namespace Flora.Services.Catalogs.Products;

public static class GuardExtensions
{
    public static void ExistsProduct(this IGuardClause guardClause, bool exists, Guid productId)
    {
        if (exists == false)
            throw new ProductNotFoundException(productId);
    }
}
