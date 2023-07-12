using BuildingBlocks.Core.Domain.Exceptions;

namespace Flora.Services.Catalogs.Categories.Exceptions.Domain;

public class CategoryDomainException : DomainException
{
    public CategoryDomainException(string message)
        : base(message) { }

    public CategoryDomainException(Guid id)
        : base($"Category with id: '{id}' not found.") { }
}
