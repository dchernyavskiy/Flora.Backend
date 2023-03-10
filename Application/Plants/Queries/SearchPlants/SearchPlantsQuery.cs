using AutoMapper;
using AutoMapper.QueryableExtensions;
using Flora.Application.Common.Interfaces;
using Flora.Application.Common.Mappings;
using Flora.Application.Common.Models;
using Flora.Domain.Entities;
using Flora.Domain.ValueObjects;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Flora.Application.Plants.Queries.SearchPlants;

public class SearchPlantBriefDto : BaseDto, IMapWith<Plant>
{
    public string PlantName { get; set; } = null!;
    public string CategoryName { get; set; } = null!;
    public Photo Photo { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<Plant, SearchPlantBriefDto>()
            .ForMember(x => x.PlantName, opts => opts.MapFrom(src => src.Name))
            .ForMember(x => x.CategoryName, opts => opts.MapFrom(src => src.Category.Name))
            .ReverseMap();
    }
}

public record SearchPlantsQuery : IRequest<Collection<SearchPlantBriefDto>>
{
    public string SearchString { get; set; }
}

public class SearchPlantsQueryHandler : IRequestHandler<SearchPlantsQuery, Collection<SearchPlantBriefDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public SearchPlantsQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<Collection<SearchPlantBriefDto>> Handle(SearchPlantsQuery request, CancellationToken cancellationToken)
    {
        return await _context.Plants
            .Include(x => x.Category)
            .Where(x => x.Name.Contains(request.SearchString))
            .ProjectTo<SearchPlantBriefDto>(_mapper.ConfigurationProvider)
            .ToCollectionAsync();
    }
}