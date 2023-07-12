using BuildingBlocks.Core.Messaging;

namespace Flora.Services.Shared.Catalogs.Products.Events.v1.Integration;

public record ProductCreatedV1(Guid Id, string Name, Guid CategoryId, string CategoryName, int Stock)
    : IntegrationEvent;
