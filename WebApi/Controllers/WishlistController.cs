using Flora.Application.Wishlists.Commands.CreateWishlist;
using Flora.Application.Wishlists.Queries.GetWishlist;
using Flora.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Flora.WebApi.Controllers;

[ApiVersionNeutral]
public class WishlistController : ApiControllerBase
{
    [HttpGet]
    public async Task<ActionResult<WishlistDto>> Get([FromQuery] GetWishlistQuery query)
    {
        return await Mediator.Send(query);
    }

    [HttpPost]
    [Authorize(Roles = nameof(Role.Buyer))]
    public async Task<ActionResult<Guid>> Create([FromBody] CreateWishlistCommand command)
    {
        return await Mediator.Send(command);
    }
}