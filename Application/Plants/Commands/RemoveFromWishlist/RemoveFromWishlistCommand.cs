using Flora.Application.Common.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;

namespace Flora.Application.Plants.Commands.RemoveFromWishlist;

public record RemoveFromWishlistCommand : IRequest
{
    public Guid PlantId { get; set; }   
}

public class RemoveFromWishlistCommandHandler : IRequestHandler<RemoveFromWishlistCommand>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUserService;
    
    public RemoveFromWishlistCommandHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }
    
    public async Task<Unit> Handle(RemoveFromWishlistCommand request, CancellationToken cancellationToken)
    {
        var wishlist = await _context.Wishlists.Include(x => x.Plants).FirstOrDefaultAsync(x => x.UserId == _currentUserService.UserId);
        var plant = await _context.Plants.FirstOrDefaultAsync(x => x.Id == request.PlantId);
        if (plant != null) wishlist?.Plants.Remove(plant);
        await _context.SaveChangesAsync(cancellationToken);
        
        return Unit.Value;
    }
}