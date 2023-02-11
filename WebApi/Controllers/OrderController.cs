using Flora.Application.Common.Models;
using Flora.Application.Orders.Commands.CreateOrder;
using Flora.Application.Orders.Queries.GetOrder;
using Flora.Application.Orders.Queries.GetOrders;
using Flora.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Flora.WebApi.Controllers;

[ApiVersionNeutral]
public class OrderController : ApiControllerBase
{
    [HttpGet]
    public async Task<ActionResult<OrderDto>> Get([FromQuery] GetOrderQuery query)
    {
        return await Mediator.Send(query);
    }

    [HttpGet]
    public async Task<ActionResult<PaginatedList<OrderBriefDto>>> GetAll([FromQuery] GetOrdersQuery query)
    {
        return await Mediator.Send(query);
    }

    [HttpPost]
    [Authorize(Roles = nameof(Role.Buyer))]
    public async Task<ActionResult<Guid>> Create([FromBody] CreateOrderCommand command)
    {
        return await Mediator.Send(command);
    }
}