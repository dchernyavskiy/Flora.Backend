
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Flora.Application.Common.Interfaces;
using Flora.Application.Common.Mappings;
using Flora.Application.Common.Models;
using Flora.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Flora.Application.Plants.Queries.GetPlants;

public class PlantBriefDto : IMapWith<Plant>
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public decimal Price { get; set; }
}

public record GetPlantsQuery : IRequest<PaginatedList<PlantBriefDto>>
{
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
};

public class GetPlantsQueryHandler : IRequestHandler<GetPlantsQuery, PaginatedList<PlantBriefDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetPlantsQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginatedList<PlantBriefDto>> Handle(GetPlantsQuery request, CancellationToken cancellationToken)
    {
        return await _context.Plants
            .ProjectTo<PlantBriefDto>(_mapper.ConfigurationProvider)
            .PaginatedListAsync(request.PageNumber, request.PageSize);
    }
}