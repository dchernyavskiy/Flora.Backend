using Flora.Domain.Entities;

namespace Flora.Application.IntegrationTests.Fixtures;

internal static class PlantFixture
{
    internal static async Task<Plant> CreateOnePlantAsync()
    {
        var category = new Category()
        {
            Id = Guid.NewGuid(),
            Name = "Category1",
            Characteristics = new List<Characteristic>()
            {
                new () { Id = Guid.NewGuid(), Name = "Char1" },
                new () { Id = Guid.NewGuid(), Name = "Char2" },
                new () { Id = Guid.NewGuid(), Name = "Char3" },
                new () { Id = Guid.NewGuid(), Name = "Char4" },
            }
        };
        
        var plant = new Plant()
        {
            Id = Guid.NewGuid(),
            CharacteristicValues = category.Characteristics.Select((x, y) => new CharacteristicValue()
            {
                Id = Guid.NewGuid(),
                Characteristic = x,
                Value = $"Val{y}",
            }).ToList(),
            DeliveryDate = DateTime.Today,
            Description = "Description",
            Name = "Plant1",
            Price = 12.2m,
        };
        
        category.Plants = new List<Plant>(){plant};
        
        await AddAsync(category);
        return plant;
    }
}