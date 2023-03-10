using Flora.Application.Common.Exceptions;
using Flora.Application.Common.Interfaces;
using Flora.Application.Plants.Commands.RemoveFromWishlist;
using Flora.Application.Wishlists.Queries.GetWishlist;
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

        var wishlistDto = await _sender.Send(new GetWishlistQuery());
        var wishlist = await _context.Wishlists
            .Include(x => x.Plants)
            .SingleAsync(x => x.Id == wishlistDto.Id, cancellationToken: cancellationToken);

        if (wishlist.Plants is null)
            (wishlist.Plants = new List<Plant>()).Add(plant);
        else if (wishlist.Plants.Any(x => x.Id == request.PlantId))
        {
            await _sender.Send(new RemoveFromWishlistCommand() { PlantId = request.PlantId });
            return false;
        }
        else
            wishlist!.Plants.Add(plant);

        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }
}