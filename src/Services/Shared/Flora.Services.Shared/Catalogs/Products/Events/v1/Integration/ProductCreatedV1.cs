using BuildingBlocks.Core.Messaging;

namespace Flora.Services.Shared.Catalogs.Products.Events.v1.Integration;

public record ProductCreatedV1(
        Guid Id,
        string Name,
        string Description,
        decimal Price,
        string ProductStatus,
        Guid CategoryId,
        string CategoryName,
        int InStock,
        string ImageUrl
    )
    : IntegrationEvent;

public record ProductDeletedV1(Guid Id)
    : IntegrationEvent;

public record ProductUpdatedV1(
    Guid Id,
    string Name,
    string Description,
    decimal Price,
    string ProductStatus,
    Guid CategoryId,
    string CategoryName,
    int InStock,
    string ImageUrl
);
