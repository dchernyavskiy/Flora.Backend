using BuildingBlocks.Core.Messaging;

namespace Flora.Services.Shared.Customers.Customers.Events.v1.Integration;

public record CustomerCreatedV1(long CustomerId) : IntegrationEvent;
