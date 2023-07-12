using BuildingBlocks.Core.CQRS.Events.Internal;
using Flora.Services.Catalogs.Products.ValueObjects;

namespace Flora.Services.Catalogs.Products.Features.DebitingProductStock.v1.Events.Domain;

public record ProductStockDebited(Guid ProductId, Stock NewStock, int DebitedQuantity) : DomainEvent;
