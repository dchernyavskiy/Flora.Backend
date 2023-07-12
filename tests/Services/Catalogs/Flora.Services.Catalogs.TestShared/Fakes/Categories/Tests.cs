using Flora.Services.Catalogs.Categories.Data;
using FluentAssertions;
using Tests.Shared.XunitCategories;

namespace Flora.Services.Catalogs.TestShared.Fakes.Categories;

public class Tests
{
    [Fact]
    [CategoryTrait(TestCategory.Unit)]
    public void CategoryFaker()
    {
        var categories = new CategoryDataSeeder.CategorySeedFaker().Generate(5);
        categories.All(x => string.IsNullOrWhiteSpace(x.Name)).Should().BeFalse();
    }
}
