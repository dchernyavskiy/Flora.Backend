using System.Linq.Dynamic.Core;
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

    public DateTime DeliveryDate { get; set; }
    public double Rate { get; set; }
}

public enum OrderBy
{
    Rate,
    Price,
    DeliveryDate
}

public record GetPlantsQuery : IRequest<PaginatedList<PlantBriefDto>>
{
    public Guid CategoryId { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public OrderBy OrderBy { get; set; } = OrderBy.Rate;
    public bool Ascending { get; set; }
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
            .Include(x => x.Category).ThenInclude(x => x.Children)
            .Include(x => x.Reviews)
            .Where(x => x.CategoryId == request.CategoryId || x.Category.ParentId == request.CategoryId ||
                        x.Category.Children.Any(x => x.Id == request.CategoryId))
            .ProjectTo<PlantBriefDto>(_mapper.ConfigurationProvider)
            .OrderBy(Enum.GetName(typeof(OrderBy), request.OrderBy) + (request.Ascending ? "" : " desc"))
            .PaginatedListAsync(request.PageNumber, request.PageSize);
    }

    private async IAsyncEnumerable<IQueryable<PlantBriefDto>> GetPlantsRecAsync(Guid categoryId)
    {
        var categories = _context.Categories
            .Include(x => x.Children)
            .Include(x => x.Plants).ThenInclude(x => x.Reviews)
            .Where(x => x.Id == categoryId);

        yield return categories.SelectMany(x => x.Plants).ProjectTo<PlantBriefDto>(_mapper.ConfigurationProvider);

        await Task.Yield();

        foreach (var child in categories.SelectMany(x => x.Children))
        await foreach (var result in GetPlantsRecAsync(child.Id))
            yield return result;
    }
}