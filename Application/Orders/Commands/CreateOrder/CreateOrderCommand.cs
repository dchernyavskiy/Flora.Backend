using AutoMapper;
using Flora.Application.Common.Exceptions;
using Flora.Application.Common.Interfaces;
using Flora.Application.Common.Mappings;
using Flora.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Flora.Application.Orders.Commands.CreateOrder;

public record CreateOrderCommand : IRequest<Guid>, IMapWith<Order>
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Phone { get; set; }
    public string Email { get; set; }
}

public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, Guid>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly IDateTime _dateTime;
    private readonly ICurrentUserService _currentUserService;

    public CreateOrderCommandHandler(IApplicationDbContext context, IMapper mapper, IDateTime dateTime,
        ICurrentUserService currentUserService)
    {
        _context = context;
        _mapper = mapper;
        _dateTime = dateTime;
        _currentUserService = currentUserService;
    }

    public async Task<Guid> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        var userId = _currentUserService.UserId;
        if (userId == Guid.Empty)
            throw new UnauthorizedAccessException();

        var entity = _mapper.Map<Order>(request);
        entity.Id = Guid.NewGuid();
        entity.OrderDate = _dateTime.Now;
        entity.UserId = userId;

        
        await _context.Orders.AddAsync(entity);
        await TransferItems(entity.Id, userId);
        await _context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }

    public async Task TransferItems(Guid orderId, Guid userId)
    {
        var basket = await _context.Baskets
            .Include(x => x.BasketItems)
            .FirstOrDefaultAsync(x => x.UserId == userId);

        if (basket == null)
            throw new NotFoundException(nameof(Basket));

        var orderItems = basket.BasketItems.Select(x => new OrderItem()
        {
            Id = Guid.NewGuid(),
            OrderId = orderId,
            PlantId = x.PlantId,
            Quantity = x.Quantity
        });
        
        _context.BasketItems.RemoveRange(basket.BasketItems);
        await _context.OrderItems.AddRangeAsync(orderItems);
    }
}