using Ardalis.GuardClauses;
using AutoMapper;
using BuildingBlocks.Abstractions.CQRS.Commands;
using BuildingBlocks.Abstractions.Mapping;
using BuildingBlocks.Core.Exception;
using Flora.Services.Catalogs.Categories.Exceptions.Domain;
using Flora.Services.Catalogs.Products.Dtos.v1;
using Flora.Services.Catalogs.Products.Features.CreatingProduct.v1.Requests;
using Flora.Services.Catalogs.Products.Models;
using Flora.Services.Catalogs.Products.ValueObjects;
using Flora.Services.Catalogs.Shared.Contracts;
using Flora.Services.Catalogs.Shared.Extensions;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Flora.Services.Catalogs.Products.Features.CreatingProduct.v1;

public record CreateProduct(
    string Name,
    decimal Price,
    int Stock,
    int RestockThreshold,
    int MaxStockThreshold,
    ProductStatus Status,
    int Width,
    int Height,
    int Depth,
    string Size,
    Guid CategoryId,
    Guid BrandId,
    string? Description = null,
    IEnumerable<CreateProductImageRequest>? Images = null
) : ITxCreateCommand<CreateProductResponse>, IMapWith<Product>
{
    public Guid Id { get; init; } = Guid.NewGuid();
}

public class CreateProductValidator : AbstractValidator<CreateProduct>
{
    public CreateProductValidator()
    {
        CascadeMode = CascadeMode.Stop;

        RuleFor(x => x.Id).NotEmpty().WithMessage("InternalCommandId must be greater than 0");

        RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required.");

        RuleFor(x => x.Price).NotEmpty().GreaterThan(0).WithMessage("Price must be greater than 0");

        RuleFor(x => x.Status).IsInEnum().WithMessage("Status is required.");

        RuleFor(x => x.Stock).NotEmpty().GreaterThan(0).WithMessage("Stock must be greater than 0");

        RuleFor(x => x.MaxStockThreshold)
            .NotEmpty()
            .GreaterThan(0)
            .WithMessage("MaxStockThreshold must be greater than 0");

        RuleFor(x => x.RestockThreshold)
            .NotEmpty()
            .GreaterThan(0)
            .WithMessage("RestockThreshold must be greater than 0");

        RuleFor(x => x.CategoryId).NotEmpty().WithMessage("CategoryId must be greater than 0");

        RuleFor(x => x.BrandId).NotEmpty().WithMessage("BrandId must be greater than 0");
    }
}

public class CreateProductHandler : ICommandHandler<CreateProduct, CreateProductResponse>
{
    private readonly ILogger<CreateProductHandler> _logger;
    private readonly IMapper _mapper;
    private readonly ICatalogDbContext _catalogDbContext;

    public CreateProductHandler(
        ICatalogDbContext catalogDbContext,
        IMapper mapper,
        ILogger<CreateProductHandler> logger
    )
    {
        _logger = Guard.Against.Null(logger, nameof(logger));
        _mapper = Guard.Against.Null(mapper, nameof(mapper));
        _catalogDbContext = Guard.Against.Null(catalogDbContext, nameof(catalogDbContext));
    }

    public async Task<CreateProductResponse> Handle(CreateProduct command, CancellationToken cancellationToken)
    {
        Guard.Against.Null(command, nameof(command));

        var images = command.Images
            ?.Select(
                x =>
                    new Image() {ImageUrl = x.ImageUrl, IsMain = x.IsMain})
            .ToList();

        var category = await _catalogDbContext.FindCategoryAsync(command.CategoryId);
        Guard.Against.NotFound(category, new CategoryDomainException(command.CategoryId));

        // await _domainEventDispatcher.DispatchAsync(cancellationToken, new Events.Domain.CreatingProduct());
        var product = Product.Create(
            command.Id,
            command.Name,
            command.Description,
            Stock.Of(command.Stock, command.RestockThreshold, command.MaxStockThreshold),
            command.Status,
            command.Price,
            category!.Id,
            images);

        await _catalogDbContext.Products.AddAsync(product, cancellationToken: cancellationToken);

        await _catalogDbContext.SaveChangesAsync(cancellationToken);

        var created = await _catalogDbContext.Products
                          .Include(x => x.Category)
                          .SingleOrDefaultAsync(x => x.Id == product.Id, cancellationToken: cancellationToken);

        var productDto = _mapper.Map<ProductDto>(created);

        _logger.LogInformation("Product a with ID: '{ProductId} created.'", command.Id);

        return new CreateProductResponse(productDto);
    }
}
