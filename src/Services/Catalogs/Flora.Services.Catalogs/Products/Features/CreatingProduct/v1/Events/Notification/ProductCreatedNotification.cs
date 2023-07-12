using BuildingBlocks.Abstractions.CQRS.Events.Internal;
using BuildingBlocks.Abstractions.Messaging;
using Flora.Services.Catalogs.Products.Features.CreatingProduct.v1.Events.Domain;

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
        // We could publish integration event to bus here
        // await _bus.PublishAsync(
        //     new Flora.Services.Shared.Catalogs.Products.Events.Integration.ProductCreatedV1(
        //         notification.InternalCommandId,
        //         notification.Name,
        //         notification.Stock,
        //         notification.CategoryName ?? "",
        //         notification.Stock),
        //     null,
        //     cancellationToken);

        return;
    }
}
