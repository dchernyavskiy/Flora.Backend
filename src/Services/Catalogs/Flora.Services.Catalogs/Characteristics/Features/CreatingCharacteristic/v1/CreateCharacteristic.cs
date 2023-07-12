using Ardalis.ApiEndpoints;
using Asp.Versioning;
using AutoMapper;
using BuildingBlocks.Abstractions.CQRS.Commands;
using BuildingBlocks.Abstractions.Mapping;
using Flora.Services.Catalogs.Characteristics.Models;
using Flora.Services.Catalogs.Shared.Contracts;
using FluentValidation;
using Hellang.Middleware.ProblemDetails;
using MediatR;
using Swashbuckle.AspNetCore.Annotations;

namespace Flora.Services.Catalogs.Characteristics.Features.CreatingCharacteristic.v1;

public record CreateCharacteristic(
    string Name,
    Guid CategoryId
) : ICreateCommand, IMapWith<Characteristic>;

public class CreateCharacteristicValidator : AbstractValidator<CreateCharacteristic>
{
    public CreateCharacteristicValidator()
    {
        RuleFor(x => x.Name).NotEmpty().WithMessage("Name have to be not empty.");
    }
}

public class CreateCharacteristicHandler : ICommandHandler<CreateCharacteristic>
{
    private readonly ICatalogDbContext _context;
    private readonly IMapper _mapper;


    public CreateCharacteristicHandler(ICatalogDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<Unit> Handle(CreateCharacteristic request, CancellationToken cancellationToken)
    {
        var entity = new Characteristic() {Name = request.Name, CategoryId = request.CategoryId};

        await _context.Characteristics.AddAsync(entity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);


        return Unit.Value;
    }
}

public class CreateCharacteristicEndpoint : EndpointBaseAsync.WithRequest<CreateCharacteristic>.WithoutResult
{
    private readonly ICommandProcessor _commandProcessor;

    public CreateCharacteristicEndpoint(ICommandProcessor commandProcessor)
    {
        _commandProcessor = commandProcessor;
    }

    [HttpPost(CharacteristicConfigs.CharacteristicPrefixUri, Name = "CreateCharacteristic")]
    [ProducesResponseType(typeof(StatusCodeResult), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(StatusCodeProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(StatusCodeProblemDetails), StatusCodes.Status400BadRequest)]
    [SwaggerOperation(
        Summary = "Create Characteristic",
        Description = "Create Characteristic",
        OperationId = "CreateCharacteristic",
        Tags = new[]
               {
                   CharacteristicConfigs.Tag
               })]
    [ApiVersion(1.0)]
    public override async Task HandleAsync(
        [FromBody] CreateCharacteristic request,
        CancellationToken cancellationToken = new CancellationToken()
    )
    {
        await _commandProcessor.SendAsync(request, cancellationToken);
    }
}
