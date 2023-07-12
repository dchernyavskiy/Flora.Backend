using BuildingBlocks.Core.Messaging;

namespace Flora.Services.Shared.Catalogs.Products.Events.v1.Integration;

public record ProductStockReplenishedV1(Guid ProductId, int NewStock, int ReplenishedQuantity) : IntegrationEvent;
