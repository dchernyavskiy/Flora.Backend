using Flora.Application.Categories.Commands.CreateCategory;
using Flora.Application.Categories.Queries.GetCategory;
using Flora.Application.Characteristics.Commands.CreateCharacteristic;
using Flora.Application.Plants.Commands.AddToWishlist;
using Flora.Application.Plants.Commands.CreatePlant;
using Flora.Application.Plants.Common;
using Flora.Application.Wishlists.Commands.CreateWishlist;
using Flora.Application.Wishlists.Queries.GetWishlist;

namespace Flora.Application.IntegrationTests.Wishlists.Queries;

public class GetWishlistTests : BaseTestFixture
{
    private Guid _productId;
    private Guid _wishlistId;

    [SetUp]
    public async Task SetUp()
    {
        var createCategoryCommand = new CreateCategoryCommand()
        {
            Name = "Category2",
            Characteristics = new List<CreateCharacteristicCommand>()
            {
                new() { Name = "Char1" },
                new() { Name = "Char2" },
                new() { Name = "Char3" },
                new() { Name = "Char4" },
            }
        };
        var categoryId = await SendAsync(createCategoryCommand);
        var getCategoryQuery = new GetCategoryQuery()
        {
            Id = categoryId
        };
        var category = await SendAsync(getCategoryQuery);
        var correctCreatePlantCommand = new CreatePlantCommand()
        {
            Name = "Plant",
            Price = 12.2m,
            Description = "A reall fsdljf asdk fkasdjf asdklf asdk fjas dkfj lasdkfj sdfj sdaklf",
            CategoryId = category.Id,
            Characteristics = category.Characteristics.Select((x, y) => new CharacteristicValueDto()
            {
                Id = Guid.NewGuid(),
                Name = $"Some{y}",
                Value = $"SomeValue{y}"
            }).ToList()
        };
        _productId = await SendAsync(correctCreatePlantCommand);
        var createWishlistCommand = new CreateWishlistCommand();
        _wishlistId = await SendAsync(createWishlistCommand);
    }

    [Test]
    public async Task GetWishlist_AuthorizedUserAndEmptyWishlist_MustReturnEmptyWishlist()
    {
        var query = new GetWishlistQuery();
        var result = await SendAsync(query);
        Assert.IsEmpty(result.Plants);
    }

    [Test]
    public async Task GetWishlist_AuthorizeUserAndFilledWishlist()
    {
        var addToWishlistCommand = new AddToWishlistCommand()
        {
            PlantId = _productId
        };
        await SendAsync(addToWishlistCommand);
        
        var query = new GetWishlistQuery();
        var result = await SendAsync(query);
        
        Assert.IsNotEmpty(result.Plants);
    }
}