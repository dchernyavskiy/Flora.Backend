
using Flora.Application.Common.Exceptions;
using Flora.Application.Common.Interfaces;
using Flora.Application.Common.Models;
using Flora.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Flora.Application.Characteristics.Commands.DeleteCharacteristic;

public record DeleteCharacteristicCommand : IRequest
{
    public Guid Id { get; set; }
}

public class DeleteCharacteristicCommandHandler : IRequestHandler<DeleteCharacteristicCommand>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public DeleteCharacteristicCommandHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<Unit> Handle(DeleteCharacteristicCommand request, CancellationToken cancellationToken)
    {
        if (_currentUserService.UserId == Guid.Empty)
            throw new UnauthorizedAccessException();
        
        var entity = await _context.Characteristics.FirstOrDefaultAsync(x => x.Id == request.Id);

        if (entity == null)
            throw new NotFoundException(nameof(Characteristic), request.Id);

        _context.Characteristics.Remove(entity);
        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}