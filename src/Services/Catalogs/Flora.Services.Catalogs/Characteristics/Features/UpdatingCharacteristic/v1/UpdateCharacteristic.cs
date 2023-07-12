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

namespace Flora.Services.Catalogs.Characteristics.Features.UpdatingCharacteristic.v1;

public record UpdateCharacteristic(
    Guid Id,
    string Name
) : IUpdateCommand;

public class UpdateCharacteristicValidator : AbstractValidator<UpdateCharacteristic>
{
    public UpdateCharacteristicValidator()
    {
    }
}

public class UpdateCharacteristicHandler : ICommandHandler<UpdateCharacteristic>
{
    private readonly ICatalogDbContext _context;
    private readonly IMapper _mapper;


    public UpdateCharacteristicHandler(ICatalogDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<Unit> Handle(UpdateCharacteristic request, CancellationToken cancellationToken)
    {
        var entity = await _context.Characteristics
                         .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken: cancellationToken);

        Guard.Against.NotFound(entity, new CharacteristicNotFoundException(request.Id));

        entity.Name = request.Name;

        _context.Characteristics.Update(entity);
        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}

public class UpdateCharacteristicEndpoint : EndpointBaseAsync.WithRequest<UpdateCharacteristic>.WithoutResult
{
    private readonly ICommandProcessor _commandProcessor;

    public UpdateCharacteristicEndpoint(ICommandProcessor commandProcessor)
    {
        _commandProcessor = commandProcessor;
    }

    [HttpPut(CharacteristicConfigs.CharacteristicPrefixUri, Name = "UpdateCharacteristic")]
    [ProducesResponseType(typeof(StatusCodeResult), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(StatusCodeProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(StatusCodeProblemDetails), StatusCodes.Status400BadRequest)]
    [SwaggerOperation(
        Summary = "Update Characteristic",
        Description = "Update Characteristic",
        OperationId = "UpdateCharacteristic",
        Tags = new[]
               {
                   CharacteristicConfigs.Tag
               })]
    [ApiVersion(1.0)]
    public override async Task HandleAsync(
        [FromBody] UpdateCharacteristic request,
        CancellationToken cancellationToken = new CancellationToken()
    )
    {
        await _commandProcessor.SendAsync(request, cancellationToken);
    }
}
