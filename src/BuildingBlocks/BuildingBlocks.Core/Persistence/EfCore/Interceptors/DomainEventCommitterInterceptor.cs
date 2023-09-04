using AutoMapper;
using BuildingBlocks.Abstractions.CQRS.Events;
using BuildingBlocks.Abstractions.CQRS.Events.Internal;
using BuildingBlocks.Core.Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace BuildingBlocks.Core.Persistence.EfCore.Interceptors;

public class DomainEventCommitterInterceptor : SaveChangesInterceptor
{
    // private readonly IDomainEventPublisher _domainEventPublisher;
    //
    // public DomainEventCommitterInterceptor(IDomainEventPublisher domainEventPublisher)
    // {
    //     _domainEventPublisher = domainEventPublisher;
    // }

    // private readonly IMediator _mediator;
    //
    // public DomainEventCommitterInterceptor(IMediator mediator)
    // {
    //     _mediator = mediator;
    // }

    // private readonly IEventProcessor _eventProcessor;
    //
    // public DomainEventCommitterInterceptor(IEventProcessor eventProcessor)
    // {
    //     _eventProcessor = eventProcessor;
    // }

    public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = new CancellationToken()
    )
    {
        foreach (var entry in eventData.Context?.ChangeTracker.Entries<Aggregate<Guid>>()!)
        {
            var entity = entry.Entity;
            foreach (var @event in entity.GetUncommittedDomainEvents())
            {
                // await _mediator.Publish(@eventData, cancellationToken);
                // await _domainEventPublisher.PublishAsync(@event, cancellationToken);
                // await _eventProcessor.PublishAsync(@event, cancellationToken);
            }

            entity.MarkUncommittedDomainEventAsCommitted();
        }

        return await base.SavingChangesAsync(eventData, result, cancellationToken);
    }
}
