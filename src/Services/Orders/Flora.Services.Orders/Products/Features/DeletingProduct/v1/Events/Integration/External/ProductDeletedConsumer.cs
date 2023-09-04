using Ardalis.GuardClauses;
using Flora.Services.Orders.Shared.Contracts;
using Flora.Services.Shared.Catalogs.Products.Events.v1.Integration;
using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace Flora.Services.Orders.Products.Features.DeletingProduct.v1.Events.Integration.External;

public class ProductDeletedConsumer : IConsumer<ProductDeletedV1>
{
    private readonly IOrderDbContext _context;

    public ProductDeletedConsumer(IOrderDbContext context)
    {
        _context = context;
    }

    public async Task Consume(ConsumeContext<ProductDeletedV1> context)
    {
        var product = await _context.Products.FirstOrDefaultAsync(x => x.Id == context.Message.Id);
        Guard.Against.Null(product);
        _context.Products.Remove(product);
        await _context.SaveChangesAsync();
    }
}
