using Flora.Application.IntegrationTests.Fixtures;
using Flora.Application.Plants.Commands.AddToWishlist;
using Flora.Application.Plants.Commands.RemoveFromWishlist;
using Flora.Domain.Entities;

namespace Flora.Application.IntegrationTests.Plants.Commands;

public class RemoveFromWishlistTests : BaseTestFixture
{
    private Plant _plant;

    [SetUp]
    public async Task SetUp()
    {
        _plant = await PlantFixture.CreateOnePlantAsync();
        var command = new AddToWishlistCommand() { PlantId = _plant.Id };
        await SendAsync(command);
    }

    [Test]
    public void RemoveFromWishlist_PassedValidPlantId_ShouldReturnTrue()
    {
        var command = new RemoveFromWishlistCommand() { PlantId = _plant.Id };
        
        Assert.DoesNotThrowAsync(() => SendAsync(command));
    }
}
