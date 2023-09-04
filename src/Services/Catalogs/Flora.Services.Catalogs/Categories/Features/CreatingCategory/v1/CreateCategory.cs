using Ardalis.ApiEndpoints;
using Asp.Versioning;
using AutoMapper;
using BuildingBlocks.Abstractions.CQRS.Commands;
using BuildingBlocks.Abstractions.Mapping;
using Flora.Services.Catalogs.Characteristics.Features.CreatingCharacteristic.v1;
using Flora.Services.Catalogs.Products.Models;
using Flora.Services.Catalogs.Shared.Contracts;
using Hellang.Middleware.ProblemDetails;
using MediatR;
using Swashbuckle.AspNetCore.Annotations;

namespace Flora.Services.Catalogs.Categories.Features.CreatingCategory.v1;

public record CreateCategory(
    string Name,
    string Description,
    string ImageUrl,
    ICollection<CreateCharacteristic> Characteristics
) : ICreateCommand, IMapWith<Category>
{
    public void Mapping(Profile profile)
    {
        profile.CreateMap<CreateCategory, Category>()
            .ForMember(
                x => x.Image,
                conf => conf.MapFrom(src => new Image() {ImageUrl = src.ImageUrl, IsMain = true}));
    }
}

public class CreateCategoryHandler : ICommandHandler<CreateCategory>
{
    private readonly ICatalogDbContext _context;
    private readonly ICommandProcessor _commandProcessor;

    public CreateCategoryHandler(ICatalogDbContext context, ICommandProcessor commandProcessor)
    {
        _context = context;
        _commandProcessor = commandProcessor;
    }

    public async Task<Unit> Handle(CreateCategory request, CancellationToken cancellationToken)
    {
        var category = new Category()
                       {
                           Name = request.Name,
                           Description = request.Description,
                           Image = new Image() {ImageUrl = request.ImageUrl, IsMain = true}
                       };

        await _context.Categories.AddAsync(category, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}

public class CreateCategoryEndpoint
    : EndpointBaseAsync.WithRequest<CreateCategory>.WithoutResult
{
    private readonly ICommandProcessor _commandProcessor;

    public CreateCategoryEndpoint(ICommandProcessor commandProcessor)
    {
        _commandProcessor = commandProcessor;
    }

    [HttpPost(CategoryConfigs.CategoriesPrefixUri, Name = "CreateCategory")]
    [ProducesResponseType(typeof(StatusCodeResult), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(StatusCodeProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(StatusCodeProblemDetails), StatusCodes.Status400BadRequest)]
    [ApiVersion(1.0)]
    [SwaggerOperation(
        Summary = "CreateCategory",
        Description = "CreateCategory",
        OperationId = "CreateCategory",
        Tags = new[]
               {
                   CategoryConfigs.Tag
               })]
    public override async Task HandleAsync(
        [FromBody] CreateCategory request,
        CancellationToken cancellationToken = new CancellationToken()
    )
    {
        await _commandProcessor.SendAsync(request, cancellationToken);
    }
}
