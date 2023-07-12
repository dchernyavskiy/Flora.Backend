using BuildingBlocks.Core.CQRS.Events.Internal;
using Flora.Services.Catalogs.Products.ValueObjects;

namespace Flora.Services.Catalogs.Products.Features.ChangingProductPrice.v1;

public record ProductPriceChanged(decimal Price) : DomainEvent;
