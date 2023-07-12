using BuildingBlocks.Core.Messaging;

namespace Flora.Services.Shared.Catalogs.Products.Events.v1.Integration;

public record ProductStockDebitedV1(Guid ProductId, int NewStock, int DebitedQuantity) : IntegrationEvent;
