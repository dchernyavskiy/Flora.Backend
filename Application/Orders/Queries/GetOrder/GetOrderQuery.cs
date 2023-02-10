
using AutoMapper;
using Flora.Application.Common.Exceptions;
using Flora.Application.Common.Interfaces;
using Flora.Application.Common.Mappings;
using Flora.Application.Common.Models;
using Flora.Application.Plants.Queries.GetPlants;
using Flora.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Flora.Application.Orders.Queries.GetOrder;

public class OrderItemDto : IMapWith<OrderItem>
{
    public int Quantity { get; set; }
    public PlantBriefDto Plant { get; set; }
}

public class OrderDto : BaseDto, IMapWith<Order>
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Phone { get; set; }
    public string Email { get; set; }
    
    public DateTime OrderDate { get; set; }
    public ICollection<OrderItemDto> OrderItems { get; set; }
}

public record GetOrderQuery : IRequest<OrderDto>
{
    public Guid Id { get; set; }
};

public class GetOrderQueryHandler : IRequestHandler<GetOrderQuery, OrderDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetOrderQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<OrderDto> Handle(GetOrderQuery request, CancellationToken cancellationToken)
    {
        var entity = await _context.Orders.FirstOrDefaultAsync(x => x.Id == request.Id);

        if (entity == null)
            throw new NotFoundException(nameof(Order), request.Id);

        return _mapper.Map<OrderDto>(entity);
    }
}