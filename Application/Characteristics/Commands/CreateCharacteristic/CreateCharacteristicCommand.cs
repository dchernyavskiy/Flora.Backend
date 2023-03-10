using AutoMapper;
using Flora.Application.Common.Interfaces;
using Flora.Application.Common.Mappings;
using Flora.Application.Plants.Commands.CreatePlant;
using Flora.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Flora.Application.Characteristics.Commands.CreateCharacteristic;

public record CreateCharacteristicCommand : IRequest<Guid>, IMapWith<Characteristic>
{
    public string Name { get; set; }
    public Guid CategoryId { get; set; }
}

public class CreateCharacteristicCommandHandler : IRequestHandler<CreateCharacteristicCommand, Guid>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public CreateCharacteristicCommandHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<Guid> Handle(CreateCharacteristicCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Characteristics.FirstOrDefaultAsync(x => x.Name.Equals(request.Name));
        if (entity == null)
        {
            entity = _mapper.Map<Characteristic>(request);
            entity.Id = Guid.NewGuid();
            await _context.Characteristics.AddAsync(entity);
            await _context.SaveChangesAsync(cancellationToken);
        }
        return entity.Id;
    }
}