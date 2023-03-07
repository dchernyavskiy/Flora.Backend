using Flora.Domain.Entities;

namespace Flora.Application.IntegrationTests.Fixtures;

internal static class CategoryFixture
{
    internal static async Task<Category> CreateOneCategoryWithoutCharacteristicsAsync()
    {
        var entity = new Category()
        {
            Id = Guid.NewGuid(),
            Name = "Category1"
        };
        await AddAsync(entity);
        return entity;
    }

    internal static async Task<Category> CreateOneCategoryWithCharacteristicsAsync()
    {
        var entity = new Category()
        {
            Id = Guid.NewGuid(),
            Name = "Category1",
            Characteristics = new List<Characteristic>()
            {
                new () { Id = Guid.NewGuid(), Name = "Char1" },
                new () { Id = Guid.NewGuid(), Name = "Char2" },
                new () { Id = Guid.NewGuid(), Name = "Char3" },
                new () { Id = Guid.NewGuid(), Name = "Char4" },
            }
        };
        await AddAsync(entity);
        return entity;
    }

    internal static async Task<Category> CreateFullCategoryWithCharacteristicsAsync()
    {
        var entity = await CreateOneCategoryWithoutCharacteristicsAsync();
        entity.Children = Enumerable.Range(0, 3).Select(x => CreateOneCategoryWithCharacteristicsAsync().Result).ToList();
        return entity;
    }
}