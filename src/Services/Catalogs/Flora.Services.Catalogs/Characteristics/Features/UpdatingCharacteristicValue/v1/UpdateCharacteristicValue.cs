using Ardalis.ApiEndpoints;
using Ardalis.GuardClauses;
using Asp.Versioning;
using AutoMapper;
using BuildingBlocks.Abstractions.CQRS.Commands;
using BuildingBlocks.Core.Exception;
using Flora.Services.Catalogs.Characteristics.Exceptions.Application;
using Flora.Services.Catalogs.Shared.Contracts;
using FluentValidation;
using Hellang.Middleware.ProblemDetails;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;

namespace Flora.Services.Catalogs.Characteristics.Features.UpdatingCharacteristicValue.v1;

public record UpdateCharacteristicValue(
    Guid Id,
    string Value
) : IUpdateCommand;

public class UpdateCharacteristicValueValidator : AbstractValidator<UpdateCharacteristicValue>
{
    public UpdateCharacteristicValueValidator()
    {
    }
}

public class UpdateCharacteristicValueHandler : ICommandHandler<UpdateCharacteristicValue>
{
    private readonly ICatalogDbContext _context;
    private readonly IMapper _mapper;


    public UpdateCharacteristicValueHandler(ICatalogDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<Unit> Handle(UpdateCharacteristicValue request, CancellationToken cancellationToken)
    {
        var entity = await _context.CharacteristicValues
                         .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken: cancellationToken);

        Guard.Against.NotFound(entity, new CharacteristicNotFoundException(request.Id));

        entity.Value = request.Value;

        _context.CharacteristicValues.Update(entity);
        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}

public class UpdateCharacteristicValueEndpoint : EndpointBaseAsync.WithRequest<UpdateCharacteristicValue>.WithoutResult
{
    private readonly ICommandProcessor _commandProcessor;

    public UpdateCharacteristicValueEndpoint(ICommandProcessor commandProcessor)
    {
        _commandProcessor = commandProcessor;
    }

    [HttpPut(CharacteristicValueConfigs.CharacteristiValuePrefixUri, Name = "UpdateCharacteristicValue")]
    [ProducesResponseType(typeof(StatusCodeResult), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(StatusCodeProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(StatusCodeProblemDetails), StatusCodes.Status400BadRequest)]
    [SwaggerOperation(
        Summary = "Update Characteristic Value",
        Description = "Update Characteristic Value",
        OperationId = "UpdateCharacteristicValue",
        Tags = new[]
               {
                   CharacteristicValueConfigs.Tag
               })]
    [ApiVersion(1.0)]
    public override async Task HandleAsync(
        [FromBody] UpdateCharacteristicValue request,
        CancellationToken cancellationToken = new CancellationToken()
    )
    {
        await _commandProcessor.SendAsync(request, cancellationToken);
    }
}
