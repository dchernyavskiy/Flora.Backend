using AutoMapper;
using Flora.Application.Common.Exceptions;
using Flora.Application.Common.Interfaces;
using Flora.Application.Common.Mappings;
using Flora.Application.Common.Models;
using Flora.Application.Plants.Queries.GetPlants;
using Flora.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Flora.Application.Baskets.Queries.GetBasket;

public class BasketItemDto : IMapWith<BasketItem>
{
    public int Quantity { get; set; }
    public PlantBriefDto Plant { get; set; }
}

public class BasketDto : BaseDto, IMapWith<Basket>
{
    public ICollection<BasketItemDto> BasketItems { get; set; }
}

public record GetBasketQuery : IRequest<BasketDto>;

public class GetBasketQueryHandler : IRequestHandler<GetBasketQuery, BasketDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly ICurrentUserService _currentUserService;
    private readonly ISender _sender;

    public GetBasketQueryHandler(IApplicationDbContext context, IMapper mapper, ICurrentUserService currentUserService,
        ISender sender)
    {
        _context = context;
        _mapper = mapper;
        _currentUserService = currentUserService;
        _sender = sender;
    }

    public async Task<BasketDto> Handle(GetBasketQuery request, CancellationToken cancellationToken)
    {
        var userId = _currentUserService.UserId;
        if (userId == Guid.Empty)
            throw new UnauthorizedAccessException();

        var entity = await _context.Baskets
            .Include(x => x.BasketItems)
            .ThenInclude(x => x.Plant)
            .FirstOrDefaultAsync(x => x.UserId == userId);

        if (entity == null)
            throw new NotFoundException(nameof(Basket));

        return _mapper.Map<BasketDto>(entity);
    }
}