using Flora.Application.Common.Models;
using Flora.Application.Plants.Commands.AddToBasket;
using Flora.Application.Plants.Commands.AddToWishlist;
using Flora.Application.Plants.Commands.CreatePlant;
using Flora.Application.Plants.Commands.DeletePlant;
using Flora.Application.Plants.Commands.RemoveFromBasket;
using Flora.Application.Plants.Commands.RemoveFromWishlist;
using Flora.Application.Plants.Commands.UpdatePlant;
using Flora.Application.Plants.Queries.GetPlant;
using Flora.Application.Plants.Queries.GetPlants;
using Flora.Application.Plants.Queries.SearchPlants;
using Flora.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Flora.WebApi.Controllers;

[ApiVersionNeutral]
public class PlantController : ApiControllerBase
{
    [HttpGet]
    public async Task<ActionResult<Collection<SearchPlantBriefDto>>> Search([FromQuery] SearchPlantsQuery query)
    {
        return await Mediator.Send(query);
    }
    [HttpGet]
    public async Task<ActionResult<PlantDto>> Get([FromQuery] GetPlantQuery query)
    {
        return await Mediator.Send(query);
    }

    [HttpGet]
    public async Task<ActionResult<PaginatedList<PlantBriefDto>>> GetAll([FromQuery] GetPlantsQuery query)
    {
        return await Mediator.Send(query);
    }

    [HttpPost]
    [Authorize(Roles = nameof(Role.Administrator))]
    public async Task<ActionResult<Guid>> Create([FromBody] CreatePlantCommand command)
    {
        return await Mediator.Send(command);
    }

    [HttpPost]
    [Authorize(Roles = nameof(Role.Buyer))]
    public async Task<ActionResult<bool>> AddToWishlist([FromBody] AddToWishlistCommand command)
    {
        return await Mediator.Send(command);
    }

    [HttpDelete]
    [Authorize(Roles = nameof(Role.Buyer))]
    public async Task<ActionResult> RemoveFromWishlist([FromBody] RemoveFromWishlistCommand command)
    {
        await Mediator.Send(command);
        return Ok();
    }

    [HttpPost]
    public async Task<ActionResult<bool>> AddToBasket([FromBody] AddToBasketCommand command)
    {
        return await Mediator.Send(command);
    }

    [HttpPost]
    public async Task<ActionResult<bool>> RemoveFromBasket([FromBody] RemoveFromBasketCommand command)
    {
        return await Mediator.Send(command);
    }


    [HttpPut]
    [Authorize(Roles = nameof(Role.Administrator))]
    public async Task<ActionResult> Update([FromBody] UpdatePlantCommand command)
    {
        await Mediator.Send(command);
        return NoContent();
    }

    [HttpDelete]
    [Authorize(Roles = nameof(Role.Administrator))]
    public async Task<ActionResult> Delete([FromBody] DeletePlantCommand command)
    {
        await Mediator.Send(command);
        return NoContent();
    }
}