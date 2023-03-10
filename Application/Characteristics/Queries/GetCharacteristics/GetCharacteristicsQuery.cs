
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Flora.Application.Common.Interfaces;
using Flora.Application.Common.Mappings;
using Flora.Application.Common.Models;
using Flora.Domain.Entities;
using MediatR;

namespace Flora.Application.Characteristics.Queries.GetCharacteristics;

public class CharacteristicBriefDto : BaseDto, IMapWith<Characteristic>
{
    public string Name { get; set; }
}

public record GetCharacteristicsQuery : IRequest<PaginatedList<CharacteristicBriefDto>>
{
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
};

public class GetCharacteristicsQueryHandler : IRequestHandler<GetCharacteristicsQuery, PaginatedList<CharacteristicBriefDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetCharacteristicsQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginatedList<CharacteristicBriefDto>> Handle(GetCharacteristicsQuery request, CancellationToken cancellationToken)
    {
        return await _context.Characteristics
            .ProjectTo<CharacteristicBriefDto>(_mapper.ConfigurationProvider)
            .PaginatedListAsync(request.PageNumber, request.PageSize);
    }
}