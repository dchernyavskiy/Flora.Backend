using Flora.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Flora.Application.Wishlists.Queries.GetWishlistCount;

public class WishlistCount
{
    public int Count { get; set; }
}

public record GetWishlistCountQuery() : IRequest<WishlistCount>;

public class GetWishlistCountQueryHandler : IRequestHandler<GetWishlistCountQuery, WishlistCount>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public GetWishlistCountQueryHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<WishlistCount> Handle(GetWishlistCountQuery request, CancellationToken cancellationToken)
    {
        var wishlist = await _context.Wishlists.Include(x => x.Plants)
            .FirstOrDefaultAsync(x => x.UserId == _currentUserService.UserId);

        if (wishlist == null)
            throw new UnauthorizedAccessException();

        return new WishlistCount() { Count = wishlist.Plants.Count };
    }
}