using Flora.Application.Common.Exceptions;
using Flora.Application.IntegrationTests.Fixtures;
using Flora.Application.Plants.Commands.DeletePlant;
using Flora.Domain.Entities;

namespace Flora.Application.IntegrationTests.Plants.Commands;

public class DeletePlantTests : BaseTestFixture
{
    private Plant _plant;

    [SetUp]
    public async Task SetUp()
    {
        _plant = await PlantFixture.CreateOnePlantAsync();
    }


    [Test]
    public void DeletePlant_NonExistent_ThrowsNotFoundException()
    {
        var deletePlantCommand = new DeletePlantCommand()
        {
            Id = Guid.Empty
        };
        Assert.ThrowsAsync<NotFoundException>(() => SendAsync(deletePlantCommand));
    }

    [Test]
    public void DeletePlant_Existent_DoesNotThrow()
    {
        var deletePlantCommand = new DeletePlantCommand()
        {
            Id = _plant.Id
        };
        Assert.DoesNotThrowAsync(() => SendAsync(deletePlantCommand));
    }
}