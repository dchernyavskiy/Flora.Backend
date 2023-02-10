using Flora.Application.Common.Exceptions;
using Flora.Application.Common.Interfaces;
using Flora.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Flora.Application.Characteristics.Commands.DeleteCharacteristicValue;


public record DeleteCharacteristicValueCommand : IRequest
{
    public Guid CharacteristicId { get; set; }
    public Guid PlantId { get; set; }
}

public class DeleteCharacteristicCommandHandler : IRequestHandler<DeleteCharacteristicValueCommand>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public DeleteCharacteristicCommandHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<Unit> Handle(DeleteCharacteristicValueCommand request, CancellationToken cancellationToken)
    {
        if (_currentUserService.UserId == Guid.Empty)
            throw new UnauthorizedAccessException();
        
        var entity = await _context.CharacteristicValues
            .FirstOrDefaultAsync(x => x.CharacteristicId == request.CharacteristicId
            && x.PlantId == request.PlantId);

        if (entity == null)
            throw new NotFoundException(nameof(Characteristic));

        _context.CharacteristicValues.Remove(entity);
        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}