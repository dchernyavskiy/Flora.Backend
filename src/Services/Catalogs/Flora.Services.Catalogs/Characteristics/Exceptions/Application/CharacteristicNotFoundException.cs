using System.Net;
using BuildingBlocks.Core.Exception.Types;

namespace Flora.Services.Catalogs.Characteristics.Exceptions.Application;

public class CharacteristicNotFoundException : AppException
{
    public CharacteristicNotFoundException(Guid id)
        : base($"Characteristic with id {id} not found.", HttpStatusCode.NotFound)
    {
    }

    public CharacteristicNotFoundException(string message, HttpStatusCode statusCode = HttpStatusCode.BadRequest)
        : base(message, statusCode)
    {
    }
}
