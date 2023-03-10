using AutoMapper;
using Flora.Application.Common.Exceptions;
using Flora.Application.Common.Interfaces;
using Flora.Application.Common.Mappings;
using Flora.Application.Common.Models;
using Flora.Application.Plants.Queries.GetPlants;
using Flora.Application.Wishlists.Commands.CreateWishlist;
using Flora.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Flora.Application.Wishlists.Queries.GetWishlist;

public class WishlistDto : BaseDto, IMapWith<Wishlist>
{
    public ICollection<PlantBriefDto> Plants { get; set; }
}

public record GetWishlistQuery : IRequest<WishlistDto>;

public class GetWishlistQueryHandler : IRequestHandler<GetWishlistQuery, WishlistDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly ICurrentUserService _currentUserService;
    private readonly ISender _sender;

    public GetWishlistQueryHandler(IApplicationDbContext context, IMapper mapper,
        ICurrentUserService currentUserService, ISender sender)
    {
        _context = context;
        _mapper = mapper;
        _currentUserService = currentUserService;
        _sender = sender;
    }

    public async Task<WishlistDto> Handle(GetWishlistQuery request, CancellationToken cancellationToken)
    {
        var userId = _currentUserService.UserId;
        if (userId == Guid.Empty)
            throw new UnauthorizedAccessException();

        var entity = await _context.Wishlists
            .Include(x => x.Plants)
            .FirstOrDefaultAsync(x => x.UserId == userId);

        if (entity == null)
        {
            await _sender.Send(new CreateWishlistCommand());
            return await _sender.Send(new GetWishlistQuery());
        }

        return _mapper.Map<WishlistDto>(entity);
    }
}