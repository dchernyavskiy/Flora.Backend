using BuildingBlocks.Core.CQRS.Events.Internal;
using Flora.Services.Catalogs.Categories;
using Flora.Services.Catalogs.Products.Models;
using Flora.Services.Catalogs.Products.ValueObjects;

namespace Flora.Services.Catalogs.Products.Features.CreatingProduct.v1.Events.Domain;

public record CreatingProduct(
    Guid Id,
    string Name,
    decimal Price,
    Stock Stock,
    ProductStatus Status,
    Category? Category,
    string? Description = null
) : DomainEvent;
