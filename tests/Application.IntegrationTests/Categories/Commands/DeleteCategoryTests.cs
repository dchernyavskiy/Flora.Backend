using Flora.Application.Categories.Commands.CreateCategory;
using Flora.Application.Categories.Commands.DeleteCategory;
using Flora.Application.Common.Exceptions;
using Flora.Application.IntegrationTests.Fixtures;
using Flora.Domain.Entities;

namespace Flora.Application.IntegrationTests.Categories.Commands;
using static Testing;
public class DeleteCategoryTests : BaseTestFixture
{
    private Category _category;

    [SetUp]
    public async Task SetUp()
    {
        _category = await CategoryFixture.CreateOneCategoryWithoutCharacteristicsAsync();
    }
    
    
    [Test]
    public void DeleteCategory_NonExistent_ThrowNotFoundException()
    {
        var command = new DeleteCategoryCommand() { Id = Guid.Empty };
        Assert.ThrowsAsync<NotFoundException>(() => SendAsync(command));
    }

    [Test]
    public void DeleteCategory_Existent_MustDeleteFromDbWithoutThrows()
    {
        var deleteCategoryCommand = new DeleteCategoryCommand() { Id = _category.Id };
        
        Assert.DoesNotThrowAsync(() => SendAsync(deleteCategoryCommand));
    }
}