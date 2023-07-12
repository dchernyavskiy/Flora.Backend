using Ardalis.ApiEndpoints;
using Ardalis.GuardClauses;
using Asp.Versioning;
using AutoMapper;
using BuildingBlocks.Abstractions.CQRS.Commands;
using BuildingBlocks.Core.Exception;
using Flora.Services.Catalogs.Characteristics.Exceptions.Application;
using Flora.Services.Catalogs.Characteristics.Features.DeletingCharacteristic.v1;
using Flora.Services.Catalogs.Shared.Contracts;
using FluentValidation;
using Hellang.Middleware.ProblemDetails;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;

namespace Flora.Services.Catalogs.Characteristics.Features.DeletingCharacteristicValue.v1;

public record DeleteCharacteristicValue(Guid Id) : IDeleteCommand<Guid>;

public class DeleteCharacteristicValueValidator : AbstractValidator<DeleteCharacteristicValue>
{
    public DeleteCharacteristicValueValidator()
    {
    }
}

public class DeleteCharacteristicValueHandler : ICommandHandler<DeleteCharacteristicValue>
{
    private readonly ICatalogDbContext _context;
    private readonly IMapper _mapper;


    public DeleteCharacteristicValueHandler(ICatalogDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<Unit> Handle(DeleteCharacteristicValue request, CancellationToken cancellationToken)
    {
        var entity = await _context.CharacteristicValues
                         .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken: cancellationToken);

        Guard.Against.NotFound(entity, new CharacteristicNotFoundException(request.Id));

        _context.CharacteristicValues.Remove(entity);
        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}

public class DeleteCharacteristicValueEndpoint : EndpointBaseAsync.WithRequest<DeleteCharacteristicValue>.WithoutResult
{
    private readonly ICommandProcessor _commandProcessor;

    public DeleteCharacteristicValueEndpoint(ICommandProcessor commandProcessor)
    {
        _commandProcessor = commandProcessor;
    }

    [HttpDelete(CharacteristicValueConfigs.CharacteristiValuePrefixUri, Name = "DeleteCharacteristicValue")]
    [ProducesResponseType(typeof(StatusCodeResult), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(StatusCodeProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(StatusCodeProblemDetails), StatusCodes.Status400BadRequest)]
    [SwaggerOperation(
        Summary = "Delete Characteristic Value",
        Description = "Delete Characteristic Value",
        OperationId = "DeleteCharacteristicValue",
        Tags = new[]
               {
                   CharacteristicValueConfigs.Tag
               })]
    [ApiVersion(1.0)]
    public override async Task HandleAsync(
        [FromBody] DeleteCharacteristicValue request,
        CancellationToken cancellationToken = new CancellationToken()
    )
    {
        await _commandProcessor.SendAsync(request, cancellationToken);
    }
}
