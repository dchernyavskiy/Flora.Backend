using Flora.Application.Characteristics.Commands.CreateCharacteristic;
using Flora.Application.Common.Exceptions;
using Flora.Application.IntegrationTests.Fixtures;
using Flora.Domain.Entities;

namespace Flora.Application.IntegrationTests.Characteristics.Commands;

public class CreateCharacteristicTests : BaseTestFixture
{
    private Category _category;

    [SetUp]
    public async Task SetUp()
    {
        _category = await CategoryFixture.CreateOneCategoryWithoutCharacteristicsAsync();
    }

    [Test]
    [TestCase(null)]
    [TestCase("")]
    [TestCase("a")]
    public void CreateCharacteristic_InvalidName_ThrowsValidationException(string name)
    {
        var command = new CreateCharacteristicCommand()
        {
            CategoryId = _category.Id,
            Name = name
        };
        Assert.ThrowsAsync<ValidationException>(() => SendAsync(command));
    }


    [Test]
    [TestCase("Valid1")]
    [TestCase("Valid2")]
    public async Task CreateCharacteristic_ValidName_MustReturnNotEmptyId(string name)
    {
        var command = new CreateCharacteristicCommand()
        {
            CategoryId = _category.Id,
            Name = name
        };
        var result = await SendAsync(command);
        Assert.AreNotEqual(Guid.Empty, result);
    }
}