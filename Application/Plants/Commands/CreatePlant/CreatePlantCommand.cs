using System.ComponentModel.DataAnnotations;
using AutoMapper;
using Flora.Application.Common.Interfaces;
using Flora.Application.Common.Mappings;
using Flora.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Flora.Application.Plants.Commands.CreatePlant;

public record CreatePlantCommand : IRequest<Guid>, IMapWith<Plant>
{
    public string Name { get; set; } = null!;
    public decimal Price { get; set; }
    public string Description { get; set; } = null!;

    public ICollection<CharacteristicDto> Characteristics { get; set; } = null!;
    public ICollection<Guid> CategoryIds { get; set; } = null!;
}

public class CreatePlantCommandHandler : IRequestHandler<CreatePlantCommand, Guid>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public CreatePlantCommandHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<Guid> Handle(CreatePlantCommand request, CancellationToken cancellationToken)
    {
        var entity = _mapper.Map<Plant>(request);
        entity.Id = Guid.NewGuid();

        var categories = await _context.Categories
            .Where(x => request.CategoryIds.Contains(x.Id))
            .ToListAsync();

        entity.Categories = categories;

        await _context.Plants.AddAsync(entity);

        await _context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}