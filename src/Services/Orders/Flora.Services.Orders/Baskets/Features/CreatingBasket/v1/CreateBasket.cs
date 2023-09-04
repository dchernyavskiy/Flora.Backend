using Ardalis.ApiEndpoints;
using Ardalis.GuardClauses;
using Asp.Versioning;
using BuildingBlocks.Abstractions.CQRS.Commands;
using BuildingBlocks.Security.Jwt;
using Flora.Services.Orders.Baskets.Features.AddingProductToBasket.v1;
using Flora.Services.Orders.Baskets.Models;
using Flora.Services.Orders.Products;
using Flora.Services.Orders.Shared.Contracts;
using Hellang.Middleware.ProblemDetails;
using Swashbuckle.AspNetCore.Annotations;

namespace Flora.Services.Orders.Baskets.Features.CreatingBasket.v1;

public record CreateBasket() : ICreateCommand<CreateBasketResponse>;

public class CreateBasketHandler : ICommandHandler<CreateBasket, CreateBasketResponse>
{
    private readonly IOrderDbContext _context;
    private readonly ISecurityContextAccessor _securityContextAccessor;

    public CreateBasketHandler(IOrderDbContext context, ISecurityContextAccessor securityContextAccessor)
    {
        _context = context;
        _securityContextAccessor = securityContextAccessor;
    }

    public async Task<CreateBasketResponse> Handle(CreateBasket request, CancellationToken cancellationToken)
    {
        var userId = Guid.Parse(_securityContextAccessor.UserId);
        Guard.Against.NullOrEmpty(userId);
        var basket = new Basket() {CustomerId = userId};
        await _context.Baskets.AddAsync(basket, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return new CreateBasketResponse(basket.Id);
    }
}

public record CreateBasketResponse(Guid BasketId);

public class CreateBasketEndpoint
    : EndpointBaseAsync.WithRequest<CreateBasket>.WithResult<CreateBasketResponse>
{
    private readonly ICommandProcessor _commandProcessor;

    public CreateBasketEndpoint(ICommandProcessor commandProcessor)
    {
        _commandProcessor = commandProcessor;
    }

    [HttpPost(BasketConfigs.BusketsPrefixUri, Name = "CreateBasket")]
    [ProducesResponseType(typeof(CreateBasketResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(StatusCodeProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(StatusCodeProblemDetails), StatusCodes.Status400BadRequest)]
    [ApiVersion(1.0)]
    [SwaggerOperation(
        Summary = "CreateBasket",
        Description = "CreateBasket",
        OperationId = "CreateBasket",
        Tags = new[]
               {
                   BasketConfigs.Tag
               })]
    public override async Task<CreateBasketResponse> HandleAsync(
        [FromBody] CreateBasket request,
        CancellationToken cancellationToken = new CancellationToken()
    )
    {
        return await _commandProcessor.SendAsync(request, cancellationToken);
    }
}
