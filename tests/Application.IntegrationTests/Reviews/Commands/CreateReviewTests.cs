using Flora.Application.Common.Exceptions;
using Flora.Application.IntegrationTests.Fixtures;
using Flora.Application.Reviews.Commands.CreateReview;
using Flora.Domain.Entities;

namespace Flora.Application.IntegrationTests.Reviews.Commands;

public class CreateReviewTests : BaseTestFixture
{
    private Plant _plant;

    [SetUp]
    public async Task SetUp()
    {
        _plant = await PlantFixture.CreateOnePlantAsync();
    }

    [Test]
    [TestCase(null, "valid@mail.com", "Tom Hines", 5)]
    [TestCase("Comment text", null, "Tom Hines", 5)]
    [TestCase("Comment text", "valid@mail.com", null, 5)]
    [TestCase("Comment text", "valid@mail.com", "Tom Hines", -5)]
    [TestCase("Comment text", "valid@mail.com", "Tom Hines", 6)]
    public async Task CreateTask_InvalidProperties_ThrowsValidationExceptions(string comment, string email,
        string fullName, int rate)
    {
        var createReviewCommand = new CreateReviewCommand()
        {
            Comment = comment,
            Email = email,
            FullName = fullName,
            PlantId = _plant.Id,
            Rate = rate
        };

        Assert.ThrowsAsync<ValidationException>(() => SendAsync(createReviewCommand));
    }

    [Test]
    [TestCase("Some text", "valid@mail.com", "Tom Hines", 5)]
    public async Task CreateTask_ValidProperties_DoesNotThrow(string comment, string email,
        string fullName, int rate)
    {
        var createReviewCommand = new CreateReviewCommand()
        {
            Comment = comment,
            Email = email,
            FullName = fullName,
            PlantId = _plant.Id,
            Rate = rate
        };

        Assert.DoesNotThrowAsync(() => SendAsync(createReviewCommand));
    }
}