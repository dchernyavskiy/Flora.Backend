using Bogus;
using BuildingBlocks.Abstractions.Persistence;
using Flora.Services.Catalogs.Products.Models;
using Flora.Services.Catalogs.Products.ValueObjects;
using Flora.Services.Catalogs.Shared.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Flora.Services.Catalogs.Products.Data;

public class ProductDataSeeder : IDataSeeder
{
    // because AutoFaker generate data also for private set and init members (not read only get) it doesn't work properly with `CustomInstantiator` and we should exclude theme one by one
    public sealed class ProductSeedFaker : Faker<Product>
    {
        public ProductSeedFaker(Guid categoryId)
        {
            CustomInstantiator(
                faker =>
                {
                    var images = Enumerable
                        .Range(0, faker.Random.Int(1, 4))
                        .Select((x, y) => new Image() {ImageUrl = faker.Image.PicsumUrl(400, 400), IsMain = y == 0})
                        .ToList();

                    return new Product()
                           {
                               Name = faker.Commerce.ProductName(),
                               Description = faker.Commerce.ProductDescription(),
                               Price = faker.PickRandom<decimal>(100, 200, 500),
                               ProductStatus = faker.PickRandom<ProductStatus>(),
                               Stock = Stock.Of(faker.Random.Int(10, 20), 5, 20),
                               CategoryId = categoryId,
                               Images = images
                           };
                });
        }
    }

    private readonly ICatalogDbContext _dbContext;

    public ProductDataSeeder(ICatalogDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task SeedAllAsync()
    {
        if (await _dbContext.Products.AnyAsync()) return;

        var categoryIds = await _dbContext.Categories.Select(x => x.Id).ToListAsync();
        foreach (var id in categoryIds)
        {
            await _dbContext.Products.AddRangeAsync(new ProductSeedFaker(id).GenerateBetween(10, 30));
        }

        await _dbContext.SaveChangesAsync();
    }

    public int Order => 3;
}
