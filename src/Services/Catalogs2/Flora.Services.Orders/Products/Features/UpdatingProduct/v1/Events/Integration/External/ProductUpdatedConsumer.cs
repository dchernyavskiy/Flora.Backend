using Ardalis.GuardClauses;
using Flora.Services.Orders.Products.Models;
using Flora.Services.Orders.Shared.Contracts;
using Flora.Services.Shared.Catalogs.Products.Events.v1.Integration;
using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace Flora.Services.Orders.Products.Features.UpdatingProduct.v1.Events.Integration.External;

public class ProductUpdatedConsumer : IConsumer<ProductUpdatedV1>
{
    private readonly IOrderDbContext _context;

    public ProductUpdatedConsumer(IOrderDbContext context)
    {
        _context = context;
    }

    public async Task Consume(ConsumeContext<ProductUpdatedV1> context)
    {
        var product = await _context.Products.FirstOrDefaultAsync(x => x.Id == context.Message.Id);
        Guard.Against.Null(product);
        product.Name = context.Message.Name;
        product.Price = context.Message.Price;
        product.ProductStatus = Enum.Parse<ProductStatus>(context.Message.ProductStatus);
        product.ImageUrl = context.Message.ImageUrl;

        _context.Products.Update(product);
        await _context.SaveChangesAsync();
    }
}
