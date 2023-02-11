using Flora.Application.Characteristics.Commands.CreateCharacteristic;
using Flora.Application.Characteristics.Commands.DeleteCharacteristic;
using Flora.Application.Characteristics.Commands.DeleteCharacteristicValue;
using Flora.Application.Characteristics.Commands.UpdateCharacteristic;
using Flora.Application.Characteristics.Queries.GetCharacteristics;
using Flora.Application.Common.Models;
using Flora.Application.Plants.Common;
using Flora.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Flora.WebApi.Controllers;

[ApiVersionNeutral]
public class CharacteristicController : ApiControllerBase
{
    [HttpGet]
    public async Task<ActionResult<PaginatedList<CharacteristicBriefDto>>> GetAll([FromQuery] GetCharacteristicsQuery query)
    {
        return await Mediator.Send(query);
    }

    [HttpPost]
    [Authorize(Roles = nameof(Role.Administrator))]
    public async Task<ActionResult<Guid>> Create([FromBody] CreateCharacteristicCommand command)
    {
        return await Mediator.Send(command);
    }

    [HttpPut]
    [Authorize(Roles = nameof(Role.Administrator))]
    public async Task<ActionResult> Update([FromBody] UpdateCharacteristicCommand command)
    {
        await Mediator.Send(command);
        return NoContent();
    }

    [HttpDelete]
    [Authorize(Roles = nameof(Role.Administrator))]
    public async Task<ActionResult> Delete([FromBody] DeleteCharacteristicCommand command)
    {
        await Mediator.Send(command);
        return NoContent();
    }
    
    [HttpDelete]
    [Authorize(Roles = nameof(Role.Administrator))]
    public async Task<ActionResult> DeleteValue([FromBody] DeleteCharacteristicValueCommand command)
    {
        await Mediator.Send(command);
        return NoContent();
    }
}