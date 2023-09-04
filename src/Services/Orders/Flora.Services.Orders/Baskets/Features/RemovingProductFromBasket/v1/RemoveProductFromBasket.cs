using Ardalis.ApiEndpoints;
using Ardalis.GuardClauses;
using Asp.Versioning;
using BuildingBlocks.Abstractions.CQRS.Commands;
using BuildingBlocks.Security.Jwt;
using Flora.Services.Orders.Products;
using Flora.Services.Orders.Shared.Contracts;
using Hellang.Middleware.ProblemDetails;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;

namespace Flora.Services.Orders.Baskets.Features.RemovingProductFromBasket.v1;

public record RemoveProductFromBasket(Guid ProductId) : ICommand;

public class RemoveProductFromBasketHandler : ICommandHandler<RemoveProductFromBasket>
{
    private readonly IOrderDbContext _context;
    private readonly ISecurityContextAccessor _securityContextAccessor;

    public RemoveProductFromBasketHandler(IOrderDbContext context, ISecurityContextAccessor securityContextAccessor)
    {
        _context = context;
        _securityContextAccessor = securityContextAccessor;
    }

    public async Task<Unit> Handle(RemoveProductFromBasket request, CancellationToken cancellationToken)
    {
        var userId = Guid.Parse(_securityContextAccessor.UserId);

        var item = await _context.Baskets
                       .Where(x => x.CustomerId == userId)
                       .SelectMany(x => x.BasketItems)
                       .Where(x => x.ProductId == request.ProductId)
                       .FirstOrDefaultAsync(cancellationToken: cancellationToken);

        Guard.Against.Null(item);

        _context.BasketItems.Remove(item);
        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}

public class RemoveProductFromBasketEndpoint
    : EndpointBaseAsync.WithRequest<RemoveProductFromBasket>.WithoutResult
{
    private readonly ICommandProcessor _commandProcessor;

    public RemoveProductFromBasketEndpoint(ICommandProcessor commandProcessor)
    {
        _commandProcessor = commandProcessor;
    }

    [HttpDelete(BasketConfigs.BusketsPrefixUri + "/remove-product-from-basket", Name = "RemoveProductFromBasket")]
    [ProducesResponseType(typeof(StatusCodeResult), StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(StatusCodeProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(StatusCodeProblemDetails), StatusCodes.Status400BadRequest)]
    [ApiVersion(1.0)]
    [SwaggerOperation(
        Summary = "RemoveProductFromBasket",
        Description = "RemoveProductFromBasket",
        OperationId = "RemoveProductFromBasket",
        Tags = new[]
               {
                   BasketConfigs.Tag
               })]
    public override async Task HandleAsync(
        [FromBody] RemoveProductFromBasket request,
        CancellationToken cancellationToken = new CancellationToken()
    )
    {
        await _commandProcessor.SendAsync(request, cancellationToken);
    }
}
