using Ardalis.ApiEndpoints;
using Ardalis.GuardClauses;
using Asp.Versioning;
using AutoMapper;
using BuildingBlocks.Abstractions.CQRS.Commands;
using BuildingBlocks.Core.Exception;
using Flora.Services.Catalogs.Characteristics.Exceptions.Application;
using Flora.Services.Catalogs.Characteristics.Models;
using Flora.Services.Catalogs.Shared.Contracts;
using FluentValidation;
using Hellang.Middleware.ProblemDetails;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;

namespace Flora.Services.Catalogs.Characteristics.Features.DeletingCharacteristic.v1;

public record DeleteCharacteristic(Guid Id) : IDeleteCommand<Guid>;

public class DeleteCharacteristicValidator : AbstractValidator<DeleteCharacteristic>
{
    public DeleteCharacteristicValidator()
    {
    }
}

public class DeleteCharacteristicHandler : ICommandHandler<DeleteCharacteristic>
{
    private readonly ICatalogDbContext _context;
    private readonly IMapper _mapper;


    public DeleteCharacteristicHandler(ICatalogDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<Unit> Handle(DeleteCharacteristic request, CancellationToken cancellationToken)
    {
        var entity = await _context.Characteristics
                         .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken: cancellationToken);

        Guard.Against.NotFound(entity, new CharacteristicNotFoundException(request.Id));

        _context.Characteristics.Remove(entity);
        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}

public class DeleteCharacteristicEndpoint : EndpointBaseAsync.WithRequest<DeleteCharacteristic>.WithoutResult
{
    private readonly ICommandProcessor _commandProcessor;

    public DeleteCharacteristicEndpoint(ICommandProcessor commandProcessor)
    {
        _commandProcessor = commandProcessor;
    }

    [HttpDelete(CharacteristicConfigs.CharacteristicPrefixUri, Name = "DeleteCharacteristic")]
    [ProducesResponseType(typeof(StatusCodeResult), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(StatusCodeProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(StatusCodeProblemDetails), StatusCodes.Status400BadRequest)]
    [SwaggerOperation(
        Summary = "Delete Characteristic",
        Description = "Delete Characteristic",
        OperationId = "DeleteCharacteristic",
        Tags = new[]
               {
                   CharacteristicConfigs.Tag
               })]
    [ApiVersion(1.0)]
    public override async Task HandleAsync(
        [FromBody] DeleteCharacteristic request,
        CancellationToken cancellationToken = new CancellationToken()
    )
    {
        await _commandProcessor.SendAsync(request, cancellationToken);
    }
}
