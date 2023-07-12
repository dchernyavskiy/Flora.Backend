using BuildingBlocks.Core.CQRS.Events.Internal;

namespace Flora.Services.Catalogs.Products.Features.ChangingRestockThreshold.v1;

public record RestockThresholdChanged(Guid ProductId, int RestockThreshold) : DomainEvent;
