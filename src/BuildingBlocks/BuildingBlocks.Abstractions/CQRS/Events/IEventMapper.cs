using BuildingBlocks.Abstractions.CQRS.Events.Internal;
using BuildingBlocks.Abstractions.Messaging;

namespace BuildingBlocks.Abstractions.CQRS.Events;

public interface IEventMapper : IIDomainNotificationEventMapper, IIntegrationEventMapper
{
}

public interface IIDomainNotificationEventMapper
{
    IReadOnlyList<IDomainNotificationEvent?>? MapToDomainNotificationEvents(IReadOnlyList<IDomainEvent> domainEvents);
    IDomainNotificationEvent? MapToDomainNotificationEvent(IDomainEvent domainEvent);

    T? MapToDomainNotificationEvent<T>(IDomainEvent domainEvent)
    where T : class, IDomainNotificationEvent
    {
        return MapToDomainNotificationEvent(domainEvent) as T;
    }
}

public interface IIntegrationEventMapper
{
    IReadOnlyList<IIntegrationEvent?>? MapToIntegrationEvents(IReadOnlyList<IDomainEvent> domainEvents);
    IIntegrationEvent? MapToIntegrationEvent(IDomainEvent domainEvent);

    T? MapToIntegrationEvent<T>(IDomainEvent domainEvent)
    where T : class, IIntegrationEvent
    {
        return MapToIntegrationEvent(domainEvent) as T;
    }
}
