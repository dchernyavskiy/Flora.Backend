using Bogus;
using BuildingBlocks.Abstractions.Persistence;
using Flora.Services.Catalogs.Characteristics.Models;
using Flora.Services.Catalogs.Products.Models;
using Flora.Services.Catalogs.Shared.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Flora.Services.Catalogs.Characteristics.Data;

public class CharacteristicDataSeeder : IDataSeeder
{
    public sealed class CharacteristicSeedFaker : Faker<Characteristic>
    {
        public CharacteristicSeedFaker(Guid categoryId)
        {
            CustomInstantiator(
                faker => new Characteristic() {Name = faker.Commerce.ProductName(), CategoryId = categoryId});
        }
    }

    private readonly ICatalogDbContext _dbContext;

    public CharacteristicDataSeeder(ICatalogDbContext dbContext)
    {
        _dbContext = dbContext;
    }


    public async Task SeedAllAsync()
    {
        if (await _dbContext.Characteristics.AnyAsync()) return;

        var categoryIds = await _dbContext.Categories.Select(x => x.Id).ToListAsync();
        foreach (var id in categoryIds)
        {
            await _dbContext.Characteristics.AddRangeAsync(new CharacteristicSeedFaker(id).GenerateBetween(4, 10));
        }

        await _dbContext.SaveChangesAsync();
    }

    public int Order => 2;
}

public class CharacteristicValueDataSeeder : IDataSeeder
{
    public sealed class CharacteristicValueSeedFaker : Faker<CharacteristicValue>
    {
        public CharacteristicValueSeedFaker(Guid productId, Guid characteristicId)
        {
            CustomInstantiator(
                faker => new CharacteristicValue()
                         {
                             Value = faker.Lorem.Word(), ProductId = productId, CharacteristicId = characteristicId
                         });
        }
    }

    private readonly ICatalogDbContext _dbContext;

    public CharacteristicValueDataSeeder(ICatalogDbContext dbContext)
    {
        _dbContext = dbContext;
    }


    public async Task SeedAllAsync()
    {
        if (await _dbContext.CharacteristicValues.AnyAsync()) return;

        var products = await _dbContext.Products
                           .Include(x => x.Category)
                           .ThenInclude(x => x!.Characteristics)
                           .ToListAsync();

        foreach (var product in products)
        {
            foreach (var characteristic in product.Category!.Characteristics)
            {
                await _dbContext.CharacteristicValues.AddRangeAsync(
                    new CharacteristicValueSeedFaker(productId: product.Id, characteristicId: characteristic.Id)
                        .Generate());
            }
        }

        await _dbContext.SaveChangesAsync();
    }

    public int Order => 4;
}
