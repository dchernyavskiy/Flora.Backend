using AutoMapper;
using Flora.Application.Categories.Queries.GetCategory;
using Flora.Application.Common.Exceptions;
using Flora.Application.Common.Interfaces;
using Flora.Application.Common.Mappings;
using Flora.Application.Common.Models;
using Flora.Application.Plants.Common;
using Flora.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Flora.Application.Plants.Queries.GetPlant;

public class PlantDto : BaseDto, IMapWith<Plant>
{
    public string Name { get; set; } = null!;
    public decimal Price { get; set; }
    public string Description { get; set; }
    public CategoryDto Category { get; set; }
    
    public ICollection<CharacteristicDto> CharacteristicValues { get; set; }
    public ICollection<Review> Reviews { get; set; }
}

public record GetPlantQuery : IRequest<PlantDto>
{
    public Guid Id { get; set; }
};

public class GetPlantQueryHandler : IRequestHandler<GetPlantQuery, PlantDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetPlantQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PlantDto> Handle(GetPlantQuery request, CancellationToken cancellationToken)
    {
        var entity = await _context.Plants.FirstOrDefaultAsync(x => x.Id == request.Id);

        if (entity == null)
            throw new NotFoundException(nameof(Plant), request.Id);

        return _mapper.Map<PlantDto>(entity);
    }
}