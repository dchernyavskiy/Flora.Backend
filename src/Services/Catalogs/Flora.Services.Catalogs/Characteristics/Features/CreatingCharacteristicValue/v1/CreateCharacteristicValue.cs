using Ardalis.ApiEndpoints;
using Asp.Versioning;
using AutoMapper;
using BuildingBlocks.Abstractions.CQRS.Commands;
using Flora.Services.Catalogs.Characteristics.Models;
using Flora.Services.Catalogs.Shared.Contracts;
using FluentValidation;
using Hellang.Middleware.ProblemDetails;
using MediatR;
using Swashbuckle.AspNetCore.Annotations;

namespace Flora.Services.Catalogs.Characteristics.Features.CreatingCharacteristicValue.v1;

public record CreateCharacteristicValue(
    string Value,
    Guid ProductId,
    Guid CharacteristicId
) : ICreateCommand;

public class CreateCharacteristicValueValidator : AbstractValidator<CreateCharacteristicValue>
{
    public CreateCharacteristicValueValidator()
    {
    }
}

public class CreateCharacteristicValueHandler : ICommandHandler<CreateCharacteristicValue>
{
    private readonly ICatalogDbContext _context;
    private readonly IMapper _mapper;


    public CreateCharacteristicValueHandler(ICatalogDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<Unit> Handle(CreateCharacteristicValue request, CancellationToken cancellationToken)
    {
        var entity = new CharacteristicValue()
                     {
                         Value = request.Value,
                         CharacteristicId = request.CharacteristicId,
                         ProductId = request.ProductId
                     };

        await _context.CharacteristicValues.AddAsync(entity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);


        return Unit.Value;
    }
}

public class CreateCharacteristicValueEndpoint : EndpointBaseAsync.WithRequest<CreateCharacteristicValue>.WithoutResult
{
    private readonly ICommandProcessor _commandProcessor;

    public CreateCharacteristicValueEndpoint(ICommandProcessor commandProcessor)
    {
        _commandProcessor = commandProcessor;
    }

    [HttpPost(CharacteristicValueConfigs.CharacteristiValuePrefixUri, Name = "CreateCharacteristicValue")]
    [ProducesResponseType(typeof(StatusCodeResult), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(StatusCodeProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(StatusCodeProblemDetails), StatusCodes.Status400BadRequest)]
    [SwaggerOperation(
        Summary = "Create Characteristic",
        Description = "Create Characteristic",
        OperationId = "CreateCharacteristicValue",
        Tags = new[]
               {
                   CharacteristicValueConfigs.Tag
               })]
    [ApiVersion(1.0)]
    public override async Task HandleAsync(
        [FromBody] CreateCharacteristicValue request,
        CancellationToken cancellationToken = new CancellationToken()
    )
    {
        await _commandProcessor.SendAsync(request, cancellationToken);
    }
}
