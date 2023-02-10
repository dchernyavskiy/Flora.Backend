using AutoMapper;
using Flora.Application.Common.Exceptions;
using Flora.Application.Common.Interfaces;
using Flora.Application.Common.Mappings;
using Flora.Application.Plants.Commands.CreatePlant;
using Flora.Application.Plants.Common;
using Flora.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Flora.Application.Plants.Commands.UpdatePlant;

public record UpdatePlantCommand : IRequest, IMapWith<Plant>
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public decimal Price { get; set; }
    public string Description { get; set; } = null!;

    public ICollection<CharacteristicDto> Characteristics { get; set; } = null!;
    public ICollection<Guid> CategoryIds { get; set; } = null!;
}

public class UpdatePlantCommandHandler : IRequestHandler<UpdatePlantCommand>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public UpdatePlantCommandHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<Unit> Handle(UpdatePlantCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Plants.FirstOrDefaultAsync(x => x.Id == request.Id);

        if (entity == null)
            throw new NotFoundException(nameof(Plant), request.Id);

        _mapper.Map(request, entity);

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}