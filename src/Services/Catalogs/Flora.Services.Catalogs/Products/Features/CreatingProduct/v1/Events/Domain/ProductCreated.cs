using BuildingBlocks.Abstractions.CQRS.Events;
using BuildingBlocks.Abstractions.CQRS.Events.Internal;
using BuildingBlocks.Abstractions.Messaging;
using BuildingBlocks.Abstractions.Messaging.PersistMessage;
using BuildingBlocks.Core.CQRS.Events.Internal;
using Flora.Services.Catalogs.Products.Features.CreatingProduct.v1.Events.Notification;
using Flora.Services.Catalogs.Products.Models;
using Flora.Services.Shared.Catalogs.Products.Events.v1.Integration;
using MediatR;

namespace Flora.Services.Catalogs.Products.Features.CreatingProduct.v1.Events.Domain;

public record ProductCreated(Product Product) : DomainEvent;

// internal class ProductCreatedHandler : IDomainEventHandler<ProductCreated>
// {
//     private readonly CatalogDbContext _dbContext;
//
//     public ProductCreatedHandler(CatalogDbContext dbContext)
//     {
//         _dbContext = dbContext;
//     }
//
//     public async Task Handle(ProductCreated notification, CancellationToken cancellationToken)
//     {
//         Guard.Against.Null(notification, nameof(notification));
//     }
// }

// Mapping domain event to integration event in domain event handler is better from mapping in command handler (for preserving our domain rule invariants).
internal class ProductCreatedDomainEventToIntegrationMappingHandler : IDomainEventHandler<ProductCreated>
{
    private readonly IIntegrationEventMapper _integrationEventMapper;
    private readonly IIDomainNotificationEventMapper _domainNotificationEventMapper;
    private readonly IMessagePersistenceService _messagePersistenceService;
    private readonly IMediator _mediator;


    public ProductCreatedDomainEventToIntegrationMappingHandler(
        IMessagePersistenceService messagePersistenceService,
        IIntegrationEventMapper integrationEventMapper,
        IIDomainNotificationEventMapper domainNotificationEventMapper,
        IMediator mediator
    )
    {
        _messagePersistenceService = messagePersistenceService;
        _integrationEventMapper = integrationEventMapper;
        _domainNotificationEventMapper = domainNotificationEventMapper;
        _mediator = mediator;
    }

    public Task Handle(ProductCreated domainEvent, CancellationToken cancellationToken)
    {
        // 1. Mapping DomainEvent To IntegrationEvent
        // 2. Save Integration Event to Outbox
        var domainNotificationEvent =
            _domainNotificationEventMapper.MapToDomainNotificationEvent<ProductCreatedNotification>(domainEvent);
        var integrationEvent = _integrationEventMapper.MapToIntegrationEvent<ProductCreatedV1>(domainEvent);
        _messagePersistenceService.AddPublishMessageAsync(
            new MessageEnvelope<ProductCreatedV1>(integrationEvent, new Dictionary<string, object?>()),
            cancellationToken);
        _mediator.Send(domainNotificationEvent, cancellationToken);
        return Task.CompletedTask;
    }
}
