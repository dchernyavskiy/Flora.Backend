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

public class ReviewDto : BaseDto, IMapWith<Review>
{
    public string Comment { get; set; }
    public string FullName { get; set; }
    public int Rate { get; set; }
    public DateTime PostDate { get; set; }
    public ICollection<ReviewDto> Children { get; set; }
}

public class PlantDto : BaseDto, IMapWith<Plant>
{
    public string Name { get; set; } = null!;
    public decimal Price { get; set; }
    public string Description { get; set; }
    public double Rate { get; set; }
    public CategoryDto Category { get; set; }

    public ICollection<CharacteristicDto> CharacteristicValues { get; set; }
    public ICollection<ReviewDto> Reviews { get; set; }
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
        var entity = await _context.Plants
            .Include(x => x.Category)
            .Include(x => x.Reviews).ThenInclude(x => x.Children)
            .Include(x => x.CharacteristicValues).ThenInclude(x => x.Characteristic)
            .FirstOrDefaultAsync(x => x.Id == request.Id);

        if (entity == null)
            throw new NotFoundException(nameof(Plant), request.Id);

        var mappedEntity = _mapper.Map<PlantDto>(entity);

        mappedEntity.Rate = mappedEntity.Reviews.Any() ? mappedEntity.Reviews.Average(x => x.Rate) : 0d;

        return mappedEntity;
    }
}