using Microsoft.Extensions.Logging;
using Bogus;
using Flora.Domain.Entities;

namespace Flora.Infrastructure.Persistence;

public class ApplicationDbContextInitializer
{
    private readonly ILogger<ApplicationDbContextInitializer> _logger;
    private readonly ApplicationDbContext _context;

    public ApplicationDbContextInitializer(ILogger<ApplicationDbContextInitializer> logger,
        ApplicationDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    public async Task SeedAsync()
    {
        try
        {
            await _context.Database.EnsureDeletedAsync();
            await _context.Database.EnsureCreatedAsync();
            await TrySeedAsync();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "An error occurred while seeding the database");
            throw;
        }
    }

    public async Task TrySeedAsync()
    {
        var categories = new Faker<Category>()
            .StrictMode(false)
            .Rules((f, c) =>
            {
                c.Id = Guid.NewGuid();
                c.Name = f.Commerce.Categories(1).FirstOrDefault()!;
                c.Children = Enumerable.Range(0, f.Random.Int(2, 4)).Select(x =>
                {
                    var category = new Category()
                    {
                        Id = Guid.NewGuid(),
                        Name = f.Commerce.Categories(1).FirstOrDefault()!,
                        Characteristics = Enumerable.Range(0, f.Random.Int(3, 10)).Select(x => new Characteristic()
                        {
                            Id = Guid.NewGuid(),
                            Name = f.Random.Word()
                        }).ToList(),
                    };
                    category.Plants = Enumerable.Range(0, f.Random.Int(4, 13)).Select(x =>
                    {
                        var plant = new Plant()
                        {
                            Id = Guid.NewGuid(),
                            Name = f.Commerce.ProductName(),
                            Description = f.Random.Words(30),
                            Price = f.Random.Decimal() * f.Random.Int(100, 1000),
                            DeliveryDate = f.Date.Past(1),
                            CharacteristicValues = category.Characteristics.Select(x => new CharacteristicValue()
                            {
                                Id = Guid.NewGuid(),
                                Characteristic = x,
                                Value = f.Random.Word()
                            }).ToList(),
                            Reviews = Enumerable.Range(0, f.Random.Int(0, 4)).Select(x => new Review()
                            {
                                Id = Guid.NewGuid(),
                                Comment = f.Random.Words(20),
                                FullName = f.Name.FullName(),
                                Email = f.Internet.Email(),
                                Rate = f.Random.Int(1, 5),
                                PostDate = f.Date.Past(1),
                                Children = Enumerable.Range(0, f.Random.Int(0, 3)).Select(x => new Review()
                                {
                                    Id = Guid.NewGuid(),
                                    Comment = f.Random.Words(20),
                                    FullName = f.Name.FullName(),
                                    Email = f.Internet.Email(),
                                    PostDate = f.Date.Past(1)
                                }).ToList()
                            }).ToList()
                        };
                        plant.Rate = plant.Reviews.Any() ? plant.Reviews.Where(y => y.Rate != null).Average(y => y.Rate!.Value) : null;
                        return plant;
                    }).ToList();
                    return category;
                }).ToList();
            })
            .Generate(12);

        await _context.Categories.AddRangeAsync(categories);
        await _context.SaveChangesAsync(CancellationToken.None);
    }
}

public static class Extensions
{
    public static T Or<T>(this T input, T output, float probability)
    {
        var random = new Random();
        if (random.NextDouble() > probability) return output;
        return input;
    }
}