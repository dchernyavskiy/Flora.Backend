using AutoMapper;
using Flora.Application.Common.Interfaces;
using Flora.Application.Common.Mappings;
using Flora.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Flora.Application.Reviews.Commands.CreateReview;

public record CreateReviewCommand : IRequest<Guid>, IMapWith<Review>
{
    public string Comment { get; set; } = null!;
    public string FullName { get; set; }
    public string Email { get; set; }
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
        var entity = _mapper.Map<Review>(request);
        entity.Id = Guid.NewGuid();
        entity.PostDate = _dateTime.Now;


        if (request.ParentId != null)
        {
            entity.PlantId = null;
            var plant = await _context.Plants
                .Include(x => x.Reviews)
                .FirstOrDefaultAsync(x => x.Id == request.PlantId);
            
            if(plant != null)
                plant.Rate = plant.Reviews.Where(x => x.Rate != null).Average(x => x.Rate!.Value);
        }
        
        await _context.Reviews.AddAsync(entity);
        await _context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}