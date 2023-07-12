using BuildingBlocks.Core.CQRS.Events.Internal;
using Flora.Services.Catalogs.Categories;
using Flora.Services.Catalogs.Products.ValueObjects;

namespace Flora.Services.Catalogs.Products.Features.ChangingProductCategory.v1.Events;

public record ProductCategoryChangedNotification(Guid CategoryId, Guid ProductId) : DomainEvent;
