using Ardalis.ApiEndpoints;
using Ardalis.GuardClauses;
using Asp.Versioning;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using BuildingBlocks.Abstractions.CQRS.Commands;
using BuildingBlocks.Abstractions.CQRS.Queries;
using BuildingBlocks.Security.Jwt;
using Flora.Services.Orders.Orders.Dtos;
using Flora.Services.Orders.Shared.Contracts;
using Hellang.Middleware.ProblemDetails;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;

namespace Flora.Services.Orders.Orders.Features.GettingOrder.v1;

public record GetOrder(Guid OrderId) : IQuery<GetOrderResponse>;

public class GetOrderHandler : IQueryHandler<GetOrder, GetOrderResponse>
{
    private readonly IOrderDbContext _context;
    private readonly ISecurityContextAccessor _securityContextAccessor;
    private readonly IMapper _mapper;

    public GetOrderHandler(ISecurityContextAccessor securityContextAccessor, IOrderDbContext context, IMapper mapper)
    {
        _securityContextAccessor = securityContextAccessor;
        _context = context;
        _mapper = mapper;
    }

    public async Task<GetOrderResponse> Handle(GetOrder request, CancellationToken cancellationToken)
    {
        var userId = Guid.Parse(_securityContextAccessor.UserId);

        var order = await _context.Orders
                        .Include(x => x.OrderItems)
                        .ThenInclude(x => x.Product)
                        .Where(x => x.CustomerId == userId)
                        .ProjectTo<OrderDto>(_mapper.ConfigurationProvider)
                        .FirstOrDefaultAsync(cancellationToken: cancellationToken);

        Guard.Against.Null(order);

        return new GetOrderResponse(order);
    }
}

public record GetOrderResponse(OrderDto OrderDto);

public class GetOrderEndpoint
    : EndpointBaseAsync.WithRequest<GetOrder>.WithResult<GetOrderResponse>
{
    private readonly IQueryProcessor _queryProcessor;

    public GetOrderEndpoint(IQueryProcessor queryProcessor)
    {
        _queryProcessor = queryProcessor;
    }

    [HttpGet(OrderConfigs.OrdersPrefixUri, Name = "GetOrder")]
    [ProducesResponseType(typeof(GetOrderResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(StatusCodeProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(StatusCodeProblemDetails), StatusCodes.Status400BadRequest)]
    [ApiVersion(1.0)]
    [SwaggerOperation(
        Summary = "GetOrder",
        Description = "GetOrder",
        OperationId = "GetOrder",
        Tags = new[]
               {
                   OrderConfigs.Tag
               })]
    public override async Task<GetOrderResponse> HandleAsync(
        [FromQuery] GetOrder request,
        CancellationToken cancellationToken = new CancellationToken()
    )
    {
        return await _queryProcessor.SendAsync(request, cancellationToken);
    }
}
