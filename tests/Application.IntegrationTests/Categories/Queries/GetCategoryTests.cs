using Flora.Application.Categories.Queries.GetCategory;
using Flora.Application.Common.Exceptions;
using Flora.Application.IntegrationTests.Fixtures;
using Flora.Domain.Entities;

namespace Flora.Application.IntegrationTests.Categories.Queries;

using static Testing;

public class GetCategoryTests : BaseTestFixture
{
    private Category _category;

    [SetUp]
    public async Task SetUp()
    {
        _category = await CategoryFixture.CreateOneCategoryWithCharacteristicsAsync();
    }

    [Test]
    public void GetCategory_EmptyDb_ShouldReturnNull()
    {
        var query = new GetCategoryQuery()
        {
            Id = Guid.NewGuid()
        };

        Assert.ThrowsAsync<NotFoundException>(() => SendAsync(query));
    }

    [Test]
    public async Task GetCategory_NotEmptyDb_ShouldReturnDto()
    {
        var query = new GetCategoryQuery()
        {
            Id = _category.Id
        };

        var result = await SendAsync(query);

        Assert.NotNull(result);
        Assert.IsNotEmpty(result.Characteristics);
    }
}