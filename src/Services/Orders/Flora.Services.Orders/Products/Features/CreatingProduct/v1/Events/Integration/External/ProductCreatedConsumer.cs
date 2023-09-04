using Flora.Services.Orders.Products.Models;
using Flora.Services.Orders.Shared.Contracts;
using Flora.Services.Shared.Catalogs.Products.Events.v1.Integration;
using MassTransit;

namespace Flora.Services.Orders.Products.Features.CreatingProduct.v1.Events.Integration.External;

public class ProductCreatedConsumer : IConsumer<ProductCreatedV1>
{
    private readonly IOrderDbContext _context;

    public ProductCreatedConsumer(IOrderDbContext context)
    {
        _context = context;
    }

    public async Task Consume(ConsumeContext<ProductCreatedV1> context)
    {
        await _context.Products
            .AddAsync(
                Product.Create(
                    context.Message.Id,
                    context.Message.ImageUrl,
                    context.Message.Name,
                    context.Message.Price,
                    Enum.Parse<ProductStatus>(context.Message.ProductStatus)));
    }
}
