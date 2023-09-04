using BuildingBlocks.Abstractions.CQRS.Events.Internal;
using BuildingBlocks.Abstractions.Messaging;
using Flora.Services.Catalogs.Products.Features.CreatingProduct.v1.Events.Domain;
using Flora.Services.Shared.Catalogs.Products.Events.v1.Integration;

namespace Flora.Services.Catalogs.Products.Features.CreatingProduct.v1.Events.Notification;

public record ProductCreatedNotification(ProductCreated DomainEvent)
    : BuildingBlocks.Core.CQRS.Events.Internal.DomainNotificationEventWrapper<ProductCreated>(DomainEvent)
{
    public Guid Id => DomainEvent.Product.Id;
    public string Name => DomainEvent.Product.Name;
    public Guid CategoryId => DomainEvent.Product.CategoryId;
    public string? CategoryName => DomainEvent.Product.Category?.Name;
    public int Stock => DomainEvent.Product.Stock.Available;
}

internal class ProductCreatedNotificationHandler : IDomainNotificationEventHandler<ProductCreatedNotification>
{
    private readonly IBus _bus;

    public ProductCreatedNotificationHandler(IBus bus)
    {
        _bus = bus;
    }

    public async Task Handle(ProductCreatedNotification notification, CancellationToken cancellationToken)
    {
        await _bus.PublishAsync(
            new ProductCreatedV1(
                notification.DomainEvent.Product.Id,
                notification.DomainEvent.Product.Name,
                notification.DomainEvent.Product.Description!,
                notification.DomainEvent.Product.Price,
                notification.DomainEvent.Product.ProductStatus.ToString(),
                notification.DomainEvent.Product.CategoryId,
                notification.DomainEvent.Product.Category?.Name ?? string.Empty,
                notification.DomainEvent.Product.Stock.Available,
                notification.DomainEvent.Product.Images.First().ImageUrl),
            null,
            cancellationToken);
    }
}
