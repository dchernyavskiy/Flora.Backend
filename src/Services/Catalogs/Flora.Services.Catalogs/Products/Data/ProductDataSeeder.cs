using Bogus;
using BuildingBlocks.Abstractions.Persistence;
using Flora.Services.Catalogs.Categories;
using Flora.Services.Catalogs.Products.Models;
using Flora.Services.Catalogs.Products.ValueObjects;
using Flora.Services.Catalogs.Shared.Contracts;
using Flora.Services.Shared.Catalogs.Products.Events.v1.Integration;
using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace Flora.Services.Catalogs.Products.Data;

public class ProductDataSeeder : IDataSeeder
{
    // because AutoFaker generate data also for private set and init members (not read only get) it doesn't work properly with `CustomInstantiator` and we should exclude theme one by one
    public sealed class ProductSeedFaker : Faker<Product>
    {
        public ProductSeedFaker(Category category)
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
                               Stock = Stock.Of(faker.Random.Int(10, 20), 5, 20),
                               ProductStatus = faker.PickRandom<ProductStatus>(),
                               Price = faker.PickRandom<decimal>(100, 200, 500),
                               Category = category,
                               Images = images
                           };
                });
        }
    }

    private readonly ICatalogDbContext _dbContext;
    private readonly IBus _bus;

    public ProductDataSeeder(ICatalogDbContext dbContext, IBus bus)
    {
        _dbContext = dbContext;
        _bus = bus;
    }

    public async Task SeedAllAsync()
    {
        if (await _dbContext.Products.AnyAsync()) return;

        var categories = await _dbContext.Categories.ToListAsync();
        foreach (var category in categories)
        {
            var products = new ProductSeedFaker(category).GenerateBetween(10, 30);
            await _dbContext.Products.AddRangeAsync(products);
            foreach (var product in products)
            {
                await _bus.Publish<ProductCreatedV1>(
                    new ProductCreatedV1(
                        product.Id,
                        product.Name,
                        product.Description,
                        product.Price,
                        product.ProductStatus.ToString(),
                        product.CategoryId,
                        product.Category.Name,
                        product.Stock.Available,
                        product.Images.First().ImageUrl));
            }
        }

        await _dbContext.SaveChangesAsync();
    }

    public int Order => 3;
}
