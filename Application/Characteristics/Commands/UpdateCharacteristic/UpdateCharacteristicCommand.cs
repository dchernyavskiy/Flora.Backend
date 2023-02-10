using AutoMapper;
using Flora.Application.Common.Exceptions;
using Flora.Application.Common.Interfaces;
using Flora.Application.Common.Mappings;
using Flora.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Flora.Application.Characteristics.Commands.UpdateCharacteristic;

public record UpdateCharacteristicCommand : IRequest, IMapWith<CharacteristicValue>
{
    public Guid Id { get; set; }
    public string Value { get; set; }
}

public class UpdateCharacteristicCommandHandler : IRequestHandler<UpdateCharacteristicCommand>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public UpdateCharacteristicCommandHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<Unit> Handle(UpdateCharacteristicCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.CharacteristicValues
            .FirstOrDefaultAsync(x => x.CharacteristicId == request.Id);

        if (entity == null)
            throw new NotFoundException(nameof(Characteristic));

        _mapper.Map(request, entity);

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}