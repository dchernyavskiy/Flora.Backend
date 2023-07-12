using Ardalis.GuardClauses;
using Flora.Services.Catalogs.Categories.Exceptions.Application;

namespace Flora.Services.Catalogs.Categories;

public static class GuardExtensions
{
    public static void ExistsCategory(this IGuardClause guardClause, bool exists, Guid categoryId)
    {
        if (exists == false)
            throw new CategoryNotFoundException(categoryId);
    }
}
