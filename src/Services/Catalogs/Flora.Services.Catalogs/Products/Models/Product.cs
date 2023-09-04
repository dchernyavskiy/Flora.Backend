using Ardalis.GuardClauses;
using BuildingBlocks.Core.Domain;
using BuildingBlocks.Core.Exception;
using Flora.Services.Catalogs.Categories;
using Flora.Services.Catalogs.Characteristics.Models;
using Flora.Services.Catalogs.Products.Exceptions.Domain;
using Flora.Services.Catalogs.Products.Features.ChangingMaxThreshold.v1;
using Flora.Services.Catalogs.Products.Features.ChangingProductCategory.v1.Events;
using Flora.Services.Catalogs.Products.Features.ChangingProductPrice.v1;
using Flora.Services.Catalogs.Products.Features.ChangingRestockThreshold.v1;
using Flora.Services.Catalogs.Products.Features.CreatingProduct.v1.Events.Domain;
using Flora.Services.Catalogs.Products.Features.DebitingProductStock.v1.Events.Domain;
using Flora.Services.Catalogs.Products.Features.ReplenishingProductStock.v1.Events.Domain;
using Flora.Services.Catalogs.Products.ValueObjects;
using LinqKit;

namespace Flora.Services.Catalogs.Products.Models;

// https://event-driven.io/en/notes_about_csharp_records_and_nullable_reference_types/
// https://enterprisecraftsmanship.com/posts/link-to-an-aggregate-reference-or-id/
// https://ardalis.com/avoid-collections-as-properties/?utm_sq=grcpqjyka3
// https://learn.microsoft.com/en-us/ef/core/modeling/constructors
// https://github.com/dotnet/efcore/issues/29940
public class Product : Aggregate<Guid>
{
    // EF
    // this constructor is needed when we have a parameter constructor that has some navigation property classes in the parameters and ef will skip it and try to find other constructor, here default constructor (maybe will fix .net 8)
    public Product()
    {
        Id = Guid.NewGuid();
    }

    public string Name { get; set; } = default!;
    public string? Description { get; set; }
    public decimal Price { get; set; } = default!;
    public ProductStatus ProductStatus { get; set; }
    public Category? Category { get; set; }
    public Guid CategoryId { get; set; } = default!;
    public Stock Stock { get; set; } = default!;
    public ICollection<Image> Images { get; set; } = Enumerable.Empty<Image>().ToList();
    public ICollection<CharacteristicValue> CharacteristicValues { get; set; } = null!;

    public static Product Create(
        Guid id,
        string name,
        string? description,
        Stock stock,
        ProductStatus status,
        decimal price,
        Guid categoryId,
        IList<Image>? images = null
    )
    {
        Guard.Against.Null(id, new ProductDomainException("Product id can not be null"));
        Guard.Against.Null(stock, new ProductDomainException("Product stock can not be null"));

        var product = new Product {Id = id, Stock = stock};

        product.ChangeName(name);
        product.ChangeDescription(description);
        product.ChangePrice(price);
        product.AddProductImages(images);
        product.ChangeStatus(status);
        product.ChangeCategory(categoryId);

        product.AddDomainEvents(new ProductCreated(product));

        return product;
    }

    public void ChangeStatus(ProductStatus status)
    {
        ProductStatus = status;
    }


    /// <summary>
    /// Sets catalog item name.
    /// </summary>
    /// <param name="name">The name to be changed.</param>
    public void ChangeName(string name)
    {
        Guard.Against.Null(name, new ProductDomainException("Product name cannot be null."));

        Name = name;
    }

    /// <summary>
    /// Sets catalog item description.
    /// </summary>
    /// <param name="description">The description to be changed.</param>
    public void ChangeDescription(string? description)
    {
        Description = description;
    }

    /// <summary>
    /// Sets catalog item price.
    /// </summary>
    /// <remarks>
    /// Raise a <see cref="ProductPriceChanged"/>.
    /// </remarks>
    /// <param name="price">The price to be changed.</param>
    public void ChangePrice(decimal price)
    {
        Guard.Against.Null(price, new ProductDomainException("Price cannot be null."));

        if (Price == price) return;

        Price = price;

        AddDomainEvents(new ProductPriceChanged(price));
    }

    /// <summary>
    /// Decrements the quantity of a particular item in inventory and ensures the restockThreshold hasn't
    /// been breached. If so, a RestockRequest is generated in CheckThreshold.
    /// </summary>
    /// <param name="quantity">The number of items to debit.</param>
    /// <returns>int: Returns the number actually removed from stock. </returns>
    public int DebitStock(int quantity)
    {
        if (quantity < 0) quantity *= -1;

        if (HasStock(quantity) == false)
        {
            throw new InsufficientStockException(
                $"Empty stock, product item '{Name}' with quantity '{quantity}' is not available.");
        }

        int removed = Math.Min(quantity, Stock.Available);

        Stock = Stock.Of(Stock.Available - removed, Stock.RestockThreshold, Stock.MaxStockThreshold);

        if (Stock.Available <= Stock.RestockThreshold)
        {
            AddDomainEvents(new ProductRestockThresholdReachedEvent(Id, Stock, quantity));
        }

        AddDomainEvents(new ProductStockDebited(Id, Stock, quantity));

        return removed;
    }

    /// <summary>
    /// Increments the quantity of a particular item in inventory.
    /// </summary>
    /// <returns>int: Returns the quantity that has been added to stock.</returns>
    /// <param name="quantity">The number of items to Replenish.</param>
    public Stock ReplenishStock(int quantity)
    {
        // we don't have enough space in the inventory
        if (Stock.Available + quantity > Stock.MaxStockThreshold)
        {
            throw new MaxStockThresholdReachedException(
                $"Max stock threshold has been reached. Max stock threshold is {Stock.MaxStockThreshold}");
        }

        Stock = Stock.Of(Stock.Available + quantity, Stock.RestockThreshold, Stock.MaxStockThreshold);

        AddDomainEvents(new ProductStockReplenished(Id, Stock, quantity));

        return Stock;
    }

    public Stock ChangeMaxStockThreshold(int maxStockThreshold)
    {
        Guard.Against.NegativeOrZero(maxStockThreshold, nameof(maxStockThreshold));

        Stock = Stock.Of(Stock.Available, Stock.RestockThreshold, maxStockThreshold);

        AddDomainEvents(new MaxThresholdChanged(Id, maxStockThreshold));

        return Stock;
    }

    public Stock ChangeRestockThreshold(int restockThreshold)
    {
        Guard.Against.NegativeOrZero(restockThreshold, nameof(restockThreshold));

        Stock = Stock.Of(Stock.Available, restockThreshold, Stock.MaxStockThreshold);

        AddDomainEvents(new RestockThresholdChanged(Id, restockThreshold));

        return Stock;
    }

    public bool HasStock(int quantity)
    {
        return Stock.Available >= quantity;
    }

    public void Activate() => ChangeStatus(ProductStatus.Available);

    public void DeActive() => ChangeStatus(ProductStatus.Unavailable);

    /// <summary>
    /// Sets category.
    /// </summary>
    /// <param name="categoryId">The categoryId to be changed.</param>
    public void ChangeCategory(Guid categoryId)
    {
        Guard.Against.Null(categoryId, new ProductDomainException("CategoryId cannot be null"));
        Guard.Against.NullOrEmpty(categoryId);

        CategoryId = categoryId;

        // add event to domain events list that will be raise during commiting transaction
        AddDomainEvents(new ProductCategoryChanged(categoryId, Id));
    }

    public void AddProductImages(IList<Image>? productImages)
    {
        productImages.ForEach(x => Images.Add(x));
    }
}
