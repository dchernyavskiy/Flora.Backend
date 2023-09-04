// using Bogus;
// using Flora.Services.Catalogs.Products.Models;
// using Flora.Services.Catalogs.Products.ValueObjects;
//
// namespace Flora.Services.Catalogs.IntegrationTests;
//
// public class UnitTest1
// {
//     [Fact]
//     [Trait("Category", "Integration")]
//     public void Test1()
//     {
//         var productFaker = new Faker<Product>().CustomInstantiator(
//             faker =>
//                 Product.Create(
//                     Guid.NewGuid(),
//                     faker.Commerce.ProductName(),
//                     Stock.Of(faker.Random.Int(10, 20), 5, 20),
//                     ProductStatus.Available,
//                     faker.Commerce.ProductDescription(),
//                     faker.PickRandom<decimal>(100, 200, 500),
//                     Guid.NewGuid()));
//
//         var s = productFaker.Generate(5);
//     }
// }
