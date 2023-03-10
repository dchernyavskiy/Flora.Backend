using System.Linq.Expressions;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Flora.Application.Common.Interfaces;
using Flora.Application.Common.Mappings;
using Flora.Application.Common.Models;
using Flora.Domain.Entities;
using Flora.Domain.ValueObjects;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Flora.Application.Categories.Queries.GetCategories;

public class CategoryBriefDto : IMapWith<Category>
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public Photo Photo { get; set; }
    public ICollection<CategoryBriefDto> Children { get; set; }
}

public record GetCategoriesQuery : IRequest<Collection<CategoryBriefDto>>;

public class GetCategoriesQueryHandler : IRequestHandler<GetCategoriesQuery, Collection<CategoryBriefDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetCategoriesQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<Collection<CategoryBriefDto>> Handle(GetCategoriesQuery request,
        CancellationToken cancellationToken)
    {
        return await _context.Categories
            .Include(x => x.Children)
            .Where(x => x.Parent == null)
            .ProjectTo<CategoryBriefDto>(_mapper.ConfigurationProvider)
            .ToCollectionAsync();
    }
}