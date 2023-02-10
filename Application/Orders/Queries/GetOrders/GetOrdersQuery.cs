
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Flora.Application.Common.Interfaces;
using Flora.Application.Common.Mappings;
using Flora.Application.Common.Models;
using Flora.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Flora.Application.Orders.Queries.GetOrders;

public class OrderBriefDto : IMapWith<Order>
{
    public Guid Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime OrderDate { get; set; }
}

public record GetOrdersQuery : IRequest<PaginatedList<OrderBriefDto>>
{
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
};

public class GetOrdersQueryHandler : IRequestHandler<GetOrdersQuery, PaginatedList<OrderBriefDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetOrdersQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginatedList<OrderBriefDto>> Handle(GetOrdersQuery request, CancellationToken cancellationToken)
    {
        return await _context.Orders
            .ProjectTo<OrderBriefDto>(_mapper.ConfigurationProvider)
            .PaginatedListAsync(request.PageNumber, request.PageSize);
    }
}