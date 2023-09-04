using Ardalis.ApiEndpoints;
using Ardalis.GuardClauses;
using Asp.Versioning;
using BuildingBlocks.Abstractions.CQRS.Commands;
using BuildingBlocks.Abstractions.Mapping;
using BuildingBlocks.Security.Jwt;
using Flora.Services.Orders.Baskets;
using Flora.Services.Orders.Orders.Dtos;
using Flora.Services.Orders.Orders.Models;
using Flora.Services.Orders.Products;
using Flora.Services.Orders.Shared.Contracts;
using Hellang.Middleware.ProblemDetails;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;

namespace Flora.Services.Orders.Orders.Features.CreatingOrder.v1;

public record CreateOrder(
    string FirstName,
    string LastName,
    string Phone,
    string Email,
    ICollection<CreateOrderItemDto> OrderItems
) : IMapWith<Order>, ICreateCommand;

public class CreateOrderHandler : ICommandHandler<CreateOrder>
{
    private readonly IOrderDbContext _context;
    private readonly ISecurityContextAccessor _securityContextAccessor;

    public CreateOrderHandler(IOrderDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(CreateOrder request, CancellationToken cancellationToken)
    {
        var userId = Guid.Parse(_securityContextAccessor.UserId);

        var basket = await _context.Baskets
                         .Include(x => x.BasketItems)
                         .FirstOrDefaultAsync(
                             x => x.CustomerId == userId,
                             cancellationToken: cancellationToken);

        Guard.Against.Null(basket);

        var order = new Order()
                    {
                        CustomerId = basket.CustomerId,
                        Email = request.Email,
                        FirstName = request.FirstName,
                        LastName = request.LastName,
                        Phone = request.Phone,
                        OrderItems = basket.BasketItems
                            .Select(x => new OrderItem() {ProductId = x.ProductId, Quantity = x.Quantity})
                            .ToList()
                    };

        await _context.Orders.AddAsync(order, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}

public class CreateOrderEndpoint
    : EndpointBaseAsync.WithRequest<CreateOrder>.WithoutResult
{
    private readonly ICommandProcessor _commandProcessor;

    public CreateOrderEndpoint(ICommandProcessor commandProcessor)
    {
        _commandProcessor = commandProcessor;
    }

    [HttpPost(OrderConfigs.OrdersPrefixUri, Name = "CreateOrder")]
    [ProducesResponseType(typeof(StatusCodeResult), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(StatusCodeProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(StatusCodeProblemDetails), StatusCodes.Status400BadRequest)]
    [ApiVersion(1.0)]
    [SwaggerOperation(
        Summary = "CreateOrder",
        Description = "CreateOrder",
        OperationId = "CreateOrder",
        Tags = new[]
               {
                   OrderConfigs.Tag
               })]
    public override async Task HandleAsync(
        [FromBody] CreateOrder request,
        CancellationToken cancellationToken = new CancellationToken()
    )
    {
        await _commandProcessor.SendAsync(request, cancellationToken);
    }
}
