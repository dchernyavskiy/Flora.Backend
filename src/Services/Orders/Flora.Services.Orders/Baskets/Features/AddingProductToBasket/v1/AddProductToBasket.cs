using Ardalis.ApiEndpoints;
using Ardalis.GuardClauses;
using Asp.Versioning;
using BuildingBlocks.Abstractions.CQRS.Commands;
using BuildingBlocks.Abstractions.CQRS.Queries;
using BuildingBlocks.Security.Jwt;
using Flora.Services.Orders.Baskets.Models;
using Flora.Services.Orders.Products;
using Flora.Services.Orders.Shared.Contracts;
using FluentValidation;
using Hellang.Middleware.ProblemDetails;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Crypto.Generators;
using Swashbuckle.AspNetCore.Annotations;

namespace Flora.Services.Orders.Baskets.Features.AddingProductToBasket.v1;

public record AddProductToBasket(Guid ProductId, int Quantity = 1) : ICreateCommand;

public class AddProductToBasketValidator : AbstractValidator<AddProductToBasket>
{
    public AddProductToBasketValidator()
    {
        RuleFor(x => x.ProductId).NotEmpty();
        RuleFor(x => x.Quantity).GreaterThanOrEqualTo(1);
    }
}

public class AddProductToBasketHandler : ICommandHandler<AddProductToBasket>
{
    private readonly IOrderDbContext _context;
    private readonly ISecurityContextAccessor _securityContextAccessor;

    public AddProductToBasketHandler(IOrderDbContext context, ISecurityContextAccessor securityContextAccessor)
    {
        _context = context;
        _securityContextAccessor = securityContextAccessor;
    }

    public async Task<Unit> Handle(AddProductToBasket request, CancellationToken cancellationToken)
    {
        var userId = Guid.Parse(_securityContextAccessor.UserId);

        var basket = await _context.Baskets.FirstOrDefaultAsync(
                         x => x.CustomerId == userId,
                         cancellationToken: cancellationToken);

        Guard.Against.Null(basket);

        var product = await _context.Products.FirstOrDefaultAsync(
                          x => x.Id == request.ProductId,
                          cancellationToken: cancellationToken);
        Guard.Against.Null(product);

        var basketItem = new BasketItem() {ProductId = request.ProductId, Quantity = request.Quantity};

        await _context.BasketItems.AddAsync(basketItem, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}

public class AddProductToBasketEndpoint
    : EndpointBaseAsync.WithRequest<AddProductToBasket>.WithoutResult
{
    private readonly ICommandProcessor _commandProcessor;

    public AddProductToBasketEndpoint(ICommandProcessor commandProcessor)
    {
        _commandProcessor = commandProcessor;
    }

    [HttpPost(BasketConfigs.BusketsPrefixUri + "/add-product-to-basket", Name = "AddProductToBasket")]
    [ProducesResponseType(typeof(StatusCodeResult), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(StatusCodeProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(StatusCodeProblemDetails), StatusCodes.Status400BadRequest)]
    [ApiVersion(1.0)]
    [SwaggerOperation(
        Summary = "Add Product To Basket",
        Description = "Add Product To Basket",
        OperationId = "AddProductToBasket",
        Tags = new[]
               {
                   BasketConfigs.Tag
               })]
    public override async Task HandleAsync(
        [FromBody] AddProductToBasket request,
        CancellationToken cancellationToken = new CancellationToken()
    )
    {
        await _commandProcessor.SendAsync(request, cancellationToken);
    }
}
