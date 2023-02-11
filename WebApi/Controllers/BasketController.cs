using Flora.Application.Baskets.Commands.CreateBasket;
using Flora.Application.Baskets.Queries.GetBasket;
using Flora.Application.Common.Models;
using Flora.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Flora.WebApi.Controllers;

[ApiVersionNeutral]
public class BasketController : ApiControllerBase
{
    [HttpGet]
    public async Task<ActionResult<BasketDto>> Get([FromQuery] GetBasketQuery query)
    {
        return await Mediator.Send(query);
    }

    [HttpPost]
    [Authorize(Roles = nameof(Role.Buyer))]
    public async Task<ActionResult<Guid>> Create([FromBody] CreateBasketCommand command)
    {
        return await Mediator.Send(command);
    }
}