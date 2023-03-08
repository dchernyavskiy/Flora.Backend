using Flora.Application.IntegrationTests.Fixtures;
using Flora.Application.Plants.Commands.AddToWishlist;
using Flora.Domain.Entities;

namespace Flora.Application.IntegrationTests.Plants.Commands;

public class AddToWishlistTests : BaseTestFixture
{
    private Plant _plant;

    [SetUp]
    public async Task SetUp()
    {
        _plant = await PlantFixture.CreateOnePlantAsync();
    }

    [Test]
    public async Task AddPlantToWishlist_PassedValidPlantId_ShouldReturnTrue()
    {
        var command = new AddToWishlistCommand()
        {
            PlantId = _plant.Id
        };

        var result = await SendAsync(command);
        
        Assert.IsTrue(result);
    }

    [Test]
    public async Task AddPlantToWishlist_AddTwice_ShouldReturnFalse()
    {
        var command = new AddToWishlistCommand()
        {
            PlantId = _plant.Id
        };

        var result = await SendAsync(command);
        
        Assert.IsTrue(result);

        result = await SendAsync(command);
        
        Assert.IsFalse(result);
    }
}