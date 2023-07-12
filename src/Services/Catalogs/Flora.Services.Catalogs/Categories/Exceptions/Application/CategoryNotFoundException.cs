using System.Net;
using BuildingBlocks.Core.Exception.Types;

namespace Flora.Services.Catalogs.Categories.Exceptions.Application;

public class CategoryNotFoundException : AppException
{
    public CategoryNotFoundException(Guid id)
        : base($"Category with id '{id}' not found.", HttpStatusCode.NotFound) { }

    public CategoryNotFoundException(string message)
        : base(message, HttpStatusCode.NotFound) { }
}
