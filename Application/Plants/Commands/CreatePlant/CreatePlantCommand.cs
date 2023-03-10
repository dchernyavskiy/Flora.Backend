using System.ComponentModel.DataAnnotations;
using AutoMapper;
using Flora.Application.Common.Interfaces;
using Flora.Application.Common.Mappings;
using Flora.Application.Plants.Common;
using Flora.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Flora.Application.Plants.Commands.CreatePlant;

public record CreatePlantCommand : IRequest<Guid>, IMapWith<Plant>
{
    public string Name { get; set; } = null!;
    public decimal Price { get; set; }
    public string Description { get; set; } = null!;

    public ICollection<CharacteristicValueDto> Characteristics { get; set; } = null!;
    public Guid CategoryId { get; set; }
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
        
        await _context.Plants.AddAsync(entity);

        await _context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}