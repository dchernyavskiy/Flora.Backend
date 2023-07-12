using BuildingBlocks.Core.Messaging;

namespace Flora.Services.Shared.Catalogs.Suppliers.Events.v1.Integration;

public record SupplierUpdatedV1(long Id, string Name) : IntegrationEvent;
