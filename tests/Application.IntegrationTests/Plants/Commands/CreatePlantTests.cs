using Flora.Application.Common.Exceptions;
using Flora.Application.IntegrationTests.Fixtures;
using Flora.Application.Plants.Commands.CreatePlant;
using Flora.Application.Plants.Common;
using Flora.Domain.Entities;

namespace Flora.Application.IntegrationTests.Plants.Commands;

using static Testing;

public class CreatePlantTests : BaseTestFixture
{
    private CreatePlantCommand _correctCreatePlantCommand;
    private Category _category;

    [SetUp]
    public async Task SetUp()
    {
        _category = await CategoryFixture.CreateOneCategoryWithCharacteristicsAsync();
        
        _correctCreatePlantCommand = new CreatePlantCommand()
        {
            Name = "Plant",
            Price = 12.2m,
            Description = "A reall fsdljf asdk fkasdjf asdklf asdk fjas dkfj lasdkfj sdfj sdaklf",
            CategoryId = _category.Id,
            Characteristics = _category.Characteristics.Select((x, y) => new CharacteristicValueDto()
            {
                Id = Guid.NewGuid(),
                Name = $"Some{y}",
                Value = $"SomeValue{y}"
            }).ToList()
        };
    }

    [Test]
    public async Task CreatePlant_AllProperties_MustReturnNotEmptyGuid()
    {
        var result = await SendAsync(_correctCreatePlantCommand);

        Assert.AreNotEqual(Guid.Empty, result);
    }

    [Test]
    public async Task CreatePlant_EmptyEntity_ThrowsValidationException()
    {
        var command = new CreatePlantCommand();
        Assert.ThrowsAsync<ValidationException>(() => SendAsync(command));
    }

    [Test]
    public async Task CreatePlant_NegativePrice_ThrowsValidationException()
    {
        var command = _correctCreatePlantCommand with { Price = -_correctCreatePlantCommand.Price };
        Assert.ThrowsAsync<ValidationException>(() => SendAsync(command));
    }

    [Test]
    public async Task CreatePlant_EmptyDescription_ThrowsValidationException()
    {
        var command = _correctCreatePlantCommand with { Description = null };
        Assert.ThrowsAsync<ValidationException>(() => SendAsync(command));
    }

    [Test]
    public async Task CreatePlant_EmptyName_ThrowsValidationException()
    {
        var command = _correctCreatePlantCommand with { Name = null };
        Assert.ThrowsAsync<ValidationException>(() => SendAsync(command));
    }
    
    [Test]
    public async Task CreatePlant_EmptyCharacteristics_ThrowsValidationException()
    {
        var command = _correctCreatePlantCommand with { Characteristics = null };
        
        var result = await SendAsync(command);
        
        Assert.AreNotEqual(Guid.Empty, result);
    }
}