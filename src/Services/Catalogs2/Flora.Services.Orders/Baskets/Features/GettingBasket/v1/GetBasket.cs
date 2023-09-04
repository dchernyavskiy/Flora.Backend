using Ardalis.ApiEndpoints;
using Ardalis.GuardClauses;
using Asp.Versioning;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using BuildingBlocks.Abstractions.CQRS.Queries;
using BuildingBlocks.Security.Jwt;
using Flora.Services.Orders.Baskets.Dtos;
using Flora.Services.Orders.Products;
using Flora.Services.Orders.Shared.Contracts;
using Hellang.Middleware.ProblemDetails;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;

namespace Flora.Services.Orders.Baskets.Features.GettingBasket.v1;

public record GetBasket() : IQuery<GetBasketResponse>;

public class GetBasketHandler : IQueryHandler<GetBasket, GetBasketResponse>
{
    private readonly IOrderDbContext _context;
    private readonly ISecurityContextAccessor _securityContextAccessor;
    private readonly IMapper _mapper;

    public GetBasketHandler(IOrderDbContext context, ISecurityContextAccessor securityContextAccessor, IMapper mapper)
    {
        _context = context;
        _securityContextAccessor = securityContextAccessor;
        _mapper = mapper;
    }

    public async Task<GetBasketResponse> Handle(GetBasket request, CancellationToken cancellationToken)
    {
        var userId = Guid.Parse(_securityContextAccessor.UserId);

        var basket = await _context.Baskets
                         .Include(x => x.BasketItems)
                         .ThenInclude(x => x.Product)
                         .Where(x => x.CustomerId == userId)
                         .ProjectTo<BasketDto>(_mapper.ConfigurationProvider)
                         .FirstOrDefaultAsync(cancellationToken: cancellationToken);

        Guard.Against.Null(basket);

        return new GetBasketResponse(basket);
    }
}

public record GetBasketResponse(BasketDto BasketDto);

public class GetBasketEndpoint
    : EndpointBaseAsync.WithRequest<GetBasket>.WithResult<GetBasketResponse>
{
    private readonly IQueryProcessor _queryProcessor;

    public GetBasketEndpoint(IQueryProcessor queryProcessor)
    {
        _queryProcessor = queryProcessor;
    }

    [HttpGet(BasketConfigs.BusketsPrefixUri, Name = "GetBasket")]
    [ProducesResponseType(typeof(GetBasketResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(StatusCodeProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(StatusCodeProblemDetails), StatusCodes.Status400BadRequest)]
    [ApiVersion(1.0)]
    [SwaggerOperation(
        Summary = "GetBasket",
        Description = "GetBasket",
        OperationId = "GetBasket",
        Tags = new[]
               {
                   BasketConfigs.Tag
               })]
    public override async Task<GetBasketResponse> HandleAsync(
        [FromQuery] GetBasket request,
        CancellationToken cancellationToken = new CancellationToken()
    )
    {
        return await _queryProcessor.SendAsync(request, cancellationToken);
    }
}
