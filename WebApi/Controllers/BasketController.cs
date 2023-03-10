using Flora.Application.Baskets.Commands.ClearBasket;
using Flora.Application.Baskets.Commands.UpdateBasket;
using Flora.Application.Baskets.Queries.GetBasket;
using Flora.Application.Baskets.Queries.GetBasketCount;
using Flora.Application.Common.Models;
using Microsoft.AspNetCore.Mvc;

namespace Flora.WebApi.Controllers;

[ApiVersionNeutral]
public class BasketController : ApiControllerBase
{
    [HttpGet]
    public async Task<ActionResult<BasketCount>> GetBasketCount([FromQuery] GetBasketCountQuery query)
    {
        return await Mediator.Send(query);
    }

    [HttpGet]
    public async Task<ActionResult<Collection<BasketItemDto>>> Get([FromQuery] GetBasketQuery query)
    {
        return await Mediator.Send(query);
    }

    [HttpPost]
    public async Task<ActionResult> Clear([FromBody] ClearBasketCommand command)
    {
        await Mediator.Send(command);
        return Ok();
    }

    [HttpPut]
    public async Task<ActionResult> Update([FromBody] UpdateBasketCommand command)
    {
        await Mediator.Send(command);
        return Ok();
    }
}