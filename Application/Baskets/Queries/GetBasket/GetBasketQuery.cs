using AutoMapper;
using AutoMapper.QueryableExtensions;
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

public record GetBasketQuery : IRequest<Collection<BasketItemDto>>;

public class GetBasketQueryHandler : IRequestHandler<GetBasketQuery, Collection<BasketItemDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly ICurrentUserService _currentUserService;
    private readonly IBasketService _basketService;

    public GetBasketQueryHandler(IApplicationDbContext context, IMapper mapper,
        ICurrentUserService currentUserService, IBasketService basketService)
    {
        _context = context;
        _mapper = mapper;
        _currentUserService = currentUserService;
        _basketService = basketService;
    }

    public async Task<Collection<BasketItemDto>> Handle(GetBasketQuery request, CancellationToken cancellationToken)
    {
        var basketItems = _basketService.GetBasketItems();
        var plantIds = basketItems.Select(x => x.PlantId);
        var collection = await _context.Plants
            .Where(x => plantIds.Contains(x.Id))
            .ToListAsync();
        return await collection.Select(x => new BasketItemDto()
            {
                Plant = _mapper.Map<PlantBriefDto>(x),
                Quantity = basketItems.First(y => y.PlantId == x.Id).Quantity
            })
            .AsQueryable()
            .ToCollectionAsync();
// var collection = await _context.Plants
        //     .Join(basketItems,
        //         plant => plant.Id,
        //         item => item.PlantId,
        //         (plant, item) => new BasketItem()
        //         {
        //             Plant = plant,
        //             Quantity = item.Quantity
        //         })
        //     .ProjectTo<BasketItemDto>(_mapper.ConfigurationProvider)
        //     .ToCollectionAsync();
        // var userId = _currentUserService.UserId;
        // if (userId == Guid.Empty)
        //     throw new UnauthorizedAccessException();
        //
        // var entity = await _context.Baskets
        //     .Include(x => x.BasketItems)
        //     .ThenInclude(x => x.Plant)
        //     .FirstOrDefaultAsync(x => x.UserId == userId);
        //
        // if (entity == null)
        //     throw new NotFoundException(nameof(Basket));
        //
        // return _mapper.Map<BasketDto>(entity);

        // return collection;
    }
}