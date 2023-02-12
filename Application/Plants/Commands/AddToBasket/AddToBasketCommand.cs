using AutoMapper;
using Flora.Application.Common.Interfaces;
using Flora.Application.Common.Mappings;
using Flora.Application.Plants.Common;
using Flora.Domain.Entities;
using MediatR;

namespace Flora.Application.Plants.Commands.AddToBasket;

// public record AddToBasketCommand : IRequest<Guid>, IMapWith<BasketItem>
// {
//     public Guid PlantId { get; set; }
//     public int Quantity { get; set; }
// }
//
// public class AddToBasketCommandHandler : IRequestHandler<AddToBasketCommand, Guid>
// {
//     private readonly IApplicationDbContext _context;
//     private readonly ICurrentUserService _currentUserService;
//     private readonly IMapper _mapper;
//     private readonly ISender _sender;
//     private readonly IHttpContextAccessor _httpContextAccessor;
//     private readonly IDataProtector _dataProtector;
//
//     public AddToBasketCommandHandler(IApplicationDbContext context, ICurrentUserService currentUserService,
//         IMapper mapper, ISender sender, IHttpContextAccessor httpContextAccessor, IDataProtector dataProtector)
//     {
//         _context = context;
//         _currentUserService = currentUserService;
//         _mapper = mapper;
//         _sender = sender;
//         _httpContextAccessor = httpContextAccessor;
//         _dataProtector = dataProtector;
//     }
//
//     public async Task<Guid> Handle(AddToBasketCommand request, CancellationToken cancellationToken)
//     {
//         var userId = _currentUserService.UserId;
//         if (userId == Guid.Empty)
//             throw new UnauthorizedAccessException();
//
//         var doesUserHaveBasket = await _context.Baskets
//             .Where(x => x.UserId == userId)
//             .AnyAsync();
//
//         Guid basketId;
//         if (!doesUserHaveBasket)
//             basketId = await _sender.Send(new CreateBasketCommand());
//         else
//             basketId = (await _context.Baskets
//                 .FirstOrDefaultAsync(x => x.UserId == userId))!.Id;
//
//
//         var basketItem = _mapper.Map<BasketItem>(request);
//         basketItem.Id = Guid.NewGuid();
//         basketItem.BasketId = basketId;
//
//         await _context.BasketItems.AddAsync(basketItem);
//         await _context.SaveChangesAsync(cancellationToken);
//
//         return basketItem.Id;
//     }
// }

public record AddToBasketCommand : BasketItemBriefDto, IRequest<bool>, IMapWith<BasketItem>;

public class AddToBasketCommandHandler : IRequestHandler<AddToBasketCommand, bool>
{
    private readonly IBasketService _basketService;
    

    public AddToBasketCommandHandler(IBasketService basketService)
    {
        _basketService = basketService;
    }

    public Task<bool> Handle(AddToBasketCommand request, CancellationToken cancellationToken)
    {
        _basketService.AddBasketItem(request);
        return Task.FromResult(true);
    }


}