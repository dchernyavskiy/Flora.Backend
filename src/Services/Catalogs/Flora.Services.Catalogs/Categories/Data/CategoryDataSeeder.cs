using System.Globalization;
using AutoBogus;
using Bogus;
using BuildingBlocks.Abstractions.Persistence;
using Flora.Services.Catalogs.Products.Models;
using Flora.Services.Catalogs.Shared.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Flora.Services.Catalogs.Categories.Data;

public class CategoryDataSeeder : IDataSeeder
{
    public sealed class CategorySeedFaker : Faker<Category>
    {
        public CategorySeedFaker()
        {
            CustomInstantiator(
                faker => new Category()
                         {
                             Name = faker.Commerce.Categories(1).First(),
                             Description = faker.Commerce.ProductDescription(),
                             Image = new Image() {ImageUrl = faker.Image.PicsumUrl(400, 400), IsMain = true},
                         });
        }
    }

    private readonly ICatalogDbContext _dbContext;

    public CategoryDataSeeder(ICatalogDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task SeedAllAsync()
    {
        if (await _dbContext.Categories.AnyAsync())
        {
            return;
        }

        var categories = new CategorySeedFaker().Generate(5);

        await _dbContext.Categories.AddRangeAsync(categories);
        await _dbContext.SaveChangesAsync();
    }

    public int Order => 1;
}
