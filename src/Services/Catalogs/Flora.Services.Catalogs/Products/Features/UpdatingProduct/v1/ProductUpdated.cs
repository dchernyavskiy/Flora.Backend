using Ardalis.GuardClauses;
using BuildingBlocks.Abstractions.CQRS.Events.Internal;
using BuildingBlocks.Core.CQRS.Events.Internal;
using BuildingBlocks.Core.Exception;
using Flora.Services.Catalogs.Products.Exceptions.Application;
using Flora.Services.Catalogs.Products.Models;
using Flora.Services.Catalogs.Shared.Data;
using Microsoft.EntityFrameworkCore;

namespace Flora.Services.Catalogs.Products.Features.UpdatingProduct.v1;

public record ProductUpdated(Product Product) : DomainEvent;

public class ProductUpdatedHandler : IDomainEventHandler<ProductUpdated>
{
    private readonly CatalogDbContext _dbContext;

    public ProductUpdatedHandler(CatalogDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task Handle(ProductUpdated notification, CancellationToken cancellationToken)
    {
        Guard.Against.Null(notification, nameof(notification));
    }
}
