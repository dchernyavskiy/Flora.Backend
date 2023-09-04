using Ardalis.GuardClauses;
using BuildingBlocks.Core.Domain;
using Flora.Services.Catalogs.Categories.Exceptions.Domain;
using Flora.Services.Catalogs.Characteristics.Models;
using Flora.Services.Catalogs.Products.Models;

namespace Flora.Services.Catalogs.Categories;

// https://stackoverflow.com/a/32354885/581476
// https://learn.microsoft.com/en-us/ef/core/modeling/constructors
// https://github.com/dotnet/efcore/issues/29940
public class Category : Aggregate<Guid>
{
    // EF
    // this constructor is needed when we have a parameter constructor that has some navigation property classes in the parameters and ef will skip it and try to find other constructor, here default constructor (maybe will fix .net 8)
    public Category()
    {
        Id = Guid.NewGuid();
    }

    public string Name { get; set; } = default!;
    public string Description { get; set; } = default!;
    public Image? Image { get; set; } = default!;

    public Guid? ParentId { get; set; }
    public Category? Parent { get; set; }
    public ICollection<Category> Children { get; set; }

    public ICollection<Product> Products { get; set; } = default!;
    public ICollection<Characteristic> Characteristics { get; set; } = default!;

    public static Category Create(Guid id, string name, string code, string description = "")
    {
        var category = new Category {Id = Guard.Against.Null(id, nameof(id))};

        category.ChangeName(name);
        category.ChangeDescription(description);

        return category;
    }

    public void ChangeName(string name)
    {
        if (string.IsNullOrWhiteSpace(name)) throw new CategoryDomainException("Name can't be white space or null.");

        Name = name;
    }

    public void ChangeDescription(string description)
    {
        if (string.IsNullOrWhiteSpace(description))
            throw new CategoryDomainException("Description can't be white space or null.");

        Description = description;
    }

    public override string ToString()
    {
        return $"{Name}";
    }
}
