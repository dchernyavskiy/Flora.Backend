using BuildingBlocks.Core.CQRS.Events.Internal;
using Flora.Services.Catalogs.Products.ValueObjects;

namespace Flora.Services.Catalogs.Products.Features.ReplenishingProductStock.v1.Events.Domain;

public record ProductStockReplenished(Guid ProductId, Stock NewStock, int ReplenishedQuantity) : DomainEvent;
