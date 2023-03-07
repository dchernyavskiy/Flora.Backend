using Flora.Application.Baskets.Queries.GetBasket;

namespace Flora.Application.IntegrationTests.Baskets;

using static Testing;

public class BasketTests : BaseTestFixture
{
    [Test]
    public async Task GetBasket_EmptyBasket_ResultMustBeEmpty()
    {
        var query = new GetBasketQuery();
        var result = await SendAsync(query);

        Assert.AreEqual(0, result.Count);
        Assert.IsEmpty(result.Items);
    }
}