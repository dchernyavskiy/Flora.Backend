using Flora.Application.Categories.Commands.UpdateCategory;
using Flora.Application.Common.Exceptions;
using Flora.Application.IntegrationTests.Fixtures;
using Flora.Domain.Entities;

namespace Flora.Application.IntegrationTests.Categories.Commands;
using static Testing;
public class UpdateCategoryTests : BaseTestFixture
{
    private Category _category;

    [SetUp]
    public async Task SetUp()
    {
        _category = await CategoryFixture.CreateOneCategoryWithoutCharacteristicsAsync();
    }

    [Test]
    public async Task UpdateCategory_InvalidName_ThrowsValidationException()
    {
        var command = new UpdateCategoryCommand();
        Assert.ThrowsAsync<ValidationException>(() => SendAsync(command));
    }

    [Test]
    public async Task UpdateCategory_NonExistent_ThrowsNotFoundException()
    {
        var command = new UpdateCategoryCommand()
        {
            Id = Guid.Empty,
            Name = "Cat1"
        };
        Assert.ThrowsAsync<NotFoundException>(() => SendAsync(command));
    }

    [Test]
    public async Task UpdateCategory_ValidName_MustUpdateWithoutThrows()
    {
        var command = new UpdateCategoryCommand()
        {
            Id = _category.Id,
            Name = "Cat11"
        };
        Assert.DoesNotThrowAsync(()=>SendAsync(command));
    }
}