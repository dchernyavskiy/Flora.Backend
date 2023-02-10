using Flora.Application.Common.Exceptions;
using Flora.Application.Common.Interfaces;
using Flora.Application.Wishlists.Commands.CreateWishlist;
using Flora.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Flora.Application.Plants.Commands.AddToWishlist;

public record AddToWishlistCommand : IRequest<bool>
{
    public Guid PlantId { get; set; }
}

public class AddToWishlistCommandHandler : IRequestHandler<AddToWishlistCommand, bool>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUserService;
    private readonly ISender _sender;

    public AddToWishlistCommandHandler(IApplicationDbContext context, ICurrentUserService currentUserService,
        ISender sender)
    {
        _context = context;
        _currentUserService = currentUserService;
        _sender = sender;
    }

    public async Task<bool> Handle(AddToWishlistCommand request, CancellationToken cancellationToken)
    {
        var userId = _currentUserService.UserId;
        if (userId == Guid.Empty)
            throw new UnauthorizedAccessException();

        var plant = await _context.Plants.FirstOrDefaultAsync(x => x.Id == request.PlantId);
        if (plant == null)
            throw new NotFoundException(nameof(Plant));

        var doesUserHaveWishlist = await _context.Wishlists
            .Where(x => x.UserId == userId)
            .AnyAsync();

        var wishlistId = Guid.Empty;
        if (!doesUserHaveWishlist)
            wishlistId = await _sender.Send(new CreateWishlistCommand());

        var wishlist = doesUserHaveWishlist
            ? await _context.Wishlists
                .Include(x => x.Plants)
                .FirstOrDefaultAsync(x => x.UserId == userId)
            : await _context.Wishlists.FirstOrDefaultAsync(x => x.Id == wishlistId);

        wishlist!.Plants.Add(plant);
        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }
}