using BuildingBlocks.Core.CQRS.Events.Internal;

namespace Flora.Services.Catalogs.Products.Features.ChangingMaxThreshold.v1;

public record MaxThresholdChanged(Guid ProductId, int MaxThreshold) : DomainEvent;
