using AutoMapper;
using Flora.Application.Categories.Queries.GetCategories;
using Flora.Application.Common.Exceptions;
using Flora.Application.Common.Interfaces;
using Flora.Application.Common.Mappings;
using Flora.Application.Common.Models;
using Flora.Application.Plants.Common;
using Flora.Domain.Entities;
using Flora.Domain.ValueObjects;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Flora.Application.Categories.Queries.GetCategory;

public class CategoryDto : BaseDto, IMapWith<Category>
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public Photo Photo { get; set; }
    public ICollection<CharacteristicDto> Characteristics { get; set; }
    public ICollection<CategoryBriefDto> Children { get; set; }
}

public class CharacteristicDto : BaseDto, IMapWith<Characteristic>
{
    public string Name { get; set; }
}

public record GetCategoryQuery : IRequest<CategoryDto>
{
    public Guid Id { get; set; }
};

public class GetCategoryQueryHandler : IRequestHandler<GetCategoryQuery, CategoryDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetCategoryQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<CategoryDto> Handle(GetCategoryQuery request, CancellationToken cancellationToken)
    {
        var entity = await _context.Categories
            .Include(x => x.Children)
            .Include(x => x.Characteristics)
            .FirstOrDefaultAsync(x => x.Id == request.Id);

        if (entity == null)
            throw new NotFoundException(nameof(Category), request.Id);

        return _mapper.Map<CategoryDto>(entity);
    }
}