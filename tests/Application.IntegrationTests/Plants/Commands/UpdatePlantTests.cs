using Flora.Application.Common.Exceptions;
using Flora.Application.IntegrationTests.Fixtures;
using Flora.Application.Plants.Commands.UpdatePlant;
using Flora.Domain.Entities;

namespace Flora.Application.IntegrationTests.Plants.Commands;
using static Testing;
public class UpdatePlantTests : BaseTestFixture
{
    private Plant _plant;

    [SetUp]
    public async Task SetUp()
    {
        _plant = await PlantFixture.CreateOnePlantAsync();
    }

    [Test]
    [TestCase(null, 12.2, "some desc")]
    [TestCase("Plant2", -12.2, "some desc")]
    [TestCase("Plant2", 12.2, null)]
    public void UpdatePlant_InvalidProperties_ThrowsValidationException(string name, decimal price, string description)
    {
        var command = new UpdatePlantCommand()
        {
            Id = _plant.Id,
            Name = name,
            Description = description
        };
        Assert.ThrowsAsync<ValidationException>(() => SendAsync(command));
    }
    
    [Test]
    public void UpdatePlant_ValidProperties_DoesNotThrow()
    {
        var command = new UpdatePlantCommand()
        {
            Id = _plant.Id,
            Name = "Plant22",
            Price = 13.3m,
            Description = "Some description"
        };
        
        Assert.DoesNotThrowAsync(() => SendAsync(command));
    }
    
}