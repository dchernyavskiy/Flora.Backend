using Flora.Application.Categories.Commands.CreateCategory;
using Flora.Application.Characteristics.Commands.CreateCharacteristic;
using FluentValidation;
using ValidationException = Flora.Application.Common.Exceptions.ValidationException;

namespace Flora.Application.IntegrationTests.Categories.Commands;
using static Testing;
public class CreateCategoryTests : BaseTestFixture
{
    [Test]
    public async Task CreateCategory_OnlyName_MustReturnNotEmptyGuid()
    {
        var command = new CreateCategoryCommand()
        {
            Name = "Category1",
        };

        var result = await SendAsync(command);
        
        Assert.AreNotEqual(Guid.Empty, result);
    }

    [Test]
    public async Task CreateCategory_WithCharacteristics_MustReturnNotEmptyGuid()
    {
        var command = new CreateCategoryCommand()
        {
            Name = "Category2",
            Characteristics = new List<CreateCharacteristicCommand>()
            {
                new CreateCharacteristicCommand() { Name = "Char1" },
                new CreateCharacteristicCommand() { Name = "Char2" },
                new CreateCharacteristicCommand() { Name = "Char3" },
                new CreateCharacteristicCommand() { Name = "Char4" },
            }
        };

        var result = await SendAsync(command);

        Assert.AreNotEqual(Guid.Empty, result);
    }

    [Test]
    public void CreateCategory_EmptyName_ThrowValidationException()
    {
        var command = new CreateCategoryCommand();
        Assert.ThrowsAsync<ValidationException>(() => SendAsync(command));
    }
}