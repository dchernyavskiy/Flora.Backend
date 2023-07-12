using Ardalis.GuardClauses;
using BuildingBlocks.Abstractions.CQRS.Commands;
using BuildingBlocks.Caching;
using BuildingBlocks.Core.Exception;
using Flora.Services.Catalogs.Categories.Exceptions.Application;
using Flora.Services.Catalogs.Products.Exceptions.Application;
using Flora.Services.Catalogs.Products.Features.GettingProductById.v1;
using Flora.Services.Catalogs.Products.Models;
using Flora.Services.Catalogs.Shared.Contracts;
using Flora.Services.Catalogs.Shared.Extensions;
using FluentValidation;
using MediatR;

namespace Flora.Services.Catalogs.Products.Features.UpdatingProduct.v1;

public record UpdateProduct(
    Guid Id,
    string Name,
    decimal Price,
    int RestockThreshold,
    int MaxStockThreshold,
    ProductStatus Status,
    int Width,
    int Height,
    int Depth,
    string Size,
    Guid CategoryId,
    Guid BrandId,
    string? Description = null
) : ITxUpdateCommand;

internal class UpdateProductValidator : AbstractValidator<UpdateProduct>
{
    public UpdateProductValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}

internal class UpdateProductInvalidateCache : InvalidateCacheRequest<UpdateProduct>
{
    public override IEnumerable<string> CacheKeys(UpdateProduct request)
    {
        yield return $"{Prefix}{nameof(GetProductById)}_{request.Id}";
    }
}

internal class UpdateProductCommandHandler : ICommandHandler<UpdateProduct>
{
    private readonly ICatalogDbContext _catalogDbContext;

    public UpdateProductCommandHandler(ICatalogDbContext catalogDbContext)
    {
        _catalogDbContext = catalogDbContext;
    }

    public async Task<Unit> Handle(UpdateProduct command, CancellationToken cancellationToken)
    {
        Guard.Against.Null(command, nameof(command));

        var product = await _catalogDbContext.FindProductByIdAsync(command.Id);
        Guard.Against.NotFound(product, new ProductNotFoundException(command.Id));

        var category = await _catalogDbContext.FindCategoryAsync(command.CategoryId);
        Guard.Against.NotFound(category, new CategoryNotFoundException(command.CategoryId));

        product!.ChangeCategory(command.CategoryId);

        product.ChangeDescription(command.Description);
        product.ChangeName(command.Name);
        product.ChangePrice(command.Price);
        product.ChangeStatus(command.Status);
        product.ChangeMaxStockThreshold(command.MaxStockThreshold);
        product.ChangeRestockThreshold(command.RestockThreshold);

        await _catalogDbContext.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}
