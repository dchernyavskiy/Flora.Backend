using AutoMapper;
using Flora.Application.Common.Interfaces;
using Flora.Application.Common.Mappings;
using Flora.Domain.Entities;
using MediatR;

namespace Flora.Application.Baskets.Commands.CreateBasket;

public record CreateBasketCommand : IRequest<Guid>, IMapWith<Basket>;

public class CreateBasketCommandHandler : IRequestHandler<CreateBasketCommand, Guid>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly ICurrentUserService _currentUserService;
    
    public CreateBasketCommandHandler(IApplicationDbContext context, IMapper mapper, ICurrentUserService currentUserService)
    {
        _context = context;
        _mapper = mapper;
        _currentUserService = currentUserService;
    }

    public async Task<Guid> Handle(CreateBasketCommand request, CancellationToken cancellationToken)
    {
        var userId = _currentUserService.UserId;
        if (userId == Guid.Empty)
            throw new UnauthorizedAccessException();
        
        var entity = _mapper.Map<Basket>(request);
        entity.Id = Guid.NewGuid();
        entity.UserId = userId;

        await _context.Baskets.AddAsync(entity);
        await _context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}