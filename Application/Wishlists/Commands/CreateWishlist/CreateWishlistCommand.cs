using AutoMapper;
using Flora.Application.Common.Interfaces;
using Flora.Application.Common.Mappings;
using Flora.Domain.Entities;
using MediatR;

namespace Flora.Application.Wishlists.Commands.CreateWishlist;

public record CreateWishlistCommand : IRequest<Guid>, IMapWith<Wishlist>;

public class CreateWishlistCommandHandler : IRequestHandler<CreateWishlistCommand, Guid>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly ICurrentUserService _currentUserService;

    public CreateWishlistCommandHandler(IApplicationDbContext context, IMapper mapper, ICurrentUserService currentUserService)
    {
        _context = context;
        _mapper = mapper;
        _currentUserService = currentUserService;
    }

    public async Task<Guid> Handle(CreateWishlistCommand request, CancellationToken cancellationToken)
    {
        var userId = _currentUserService.UserId;
        if (userId == Guid.Empty)
            throw new UnauthorizedAccessException();
        
        var entity = _mapper.Map<Wishlist>(request);
        entity.Id = Guid.NewGuid();
        entity.UserId = userId;

        await _context.Wishlists.AddAsync(entity);
        await _context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}