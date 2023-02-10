using AutoMapper;
using Flora.Application.Common.Interfaces;
using Flora.Application.Common.Mappings;
using Flora.Domain.Entities;
using MediatR;

namespace Flora.Application.Reviews.Commands.CreateReview;

public record CreateReviewCommand : IRequest<Guid>, IMapWith<Review>
{
    public string Message { get; set; } = null!;
    public int Rate { get; set; } = 0;
    public Guid? ParentId { get; set; } = null;
    public Guid PlantId { get; set; }
}

public class CreateReviewCommandHandler : IRequestHandler<CreateReviewCommand, Guid>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly ICurrentUserService _currentUserService;
    private readonly IDateTime _dateTime;
    
    public CreateReviewCommandHandler(IApplicationDbContext context, IMapper mapper, ICurrentUserService currentUserService, IDateTime dateTime)
    {
        _context = context;
        _mapper = mapper;
        _currentUserService = currentUserService;
        _dateTime = dateTime;
    }

    public async Task<Guid> Handle(CreateReviewCommand request, CancellationToken cancellationToken)
    {
        var userId = _currentUserService.UserId;
        if (userId == Guid.Empty)
            throw new UnauthorizedAccessException();

        var entity = _mapper.Map<Review>(request);
        entity.Id = Guid.NewGuid();
        entity.UserId = userId;
        entity.PostDate = _dateTime.Now;
        entity.UserFirstName = _currentUserService.FirstName;
        entity.UserLastName = _currentUserService.LastName;

        await _context.Reviews.AddAsync(entity);
        await _context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}