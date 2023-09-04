using Ardalis.ApiEndpoints;
using Ardalis.GuardClauses;
using Asp.Versioning;
using BuildingBlocks.Abstractions.CQRS.Commands;
using Flora.Services.Catalogs.Characteristics;
using Flora.Services.Catalogs.Shared.Contracts;
using Hellang.Middleware.ProblemDetails;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;

namespace Flora.Services.Catalogs.Categories.Features.UpdatingCategory.v1;

public record UpdateCategory(Guid Id, string Name, string Description, string ImageUrl) : IUpdateCommand;

public class UpdateCategoryHandler : ICommandHandler<UpdateCategory>
{
    private readonly ICatalogDbContext _context;

    public UpdateCategoryHandler(ICatalogDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(UpdateCategory request, CancellationToken cancellationToken)
    {
        var category = await _context.Categories
                           .Include(x => x.Image)
                           .FirstOrDefaultAsync(
                               x => x.Id == request.Id,
                               cancellationToken: cancellationToken);
        Guard.Against.Null(category);

        if (!string.IsNullOrEmpty(request.Name)) category.Name = request.Name;
        if (!string.IsNullOrEmpty(request.Description)) category.Description = request.Description;
        if (!string.IsNullOrEmpty(request.ImageUrl)) category.Image.ImageUrl = request.ImageUrl;

        _context.Categories.Update(category);
        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}

public class UpdateCategoryEndpoint : EndpointBaseAsync.WithRequest<UpdateCategory>.WithoutResult
{
    private readonly ICommandProcessor _commandProcessor;

    public UpdateCategoryEndpoint(ICommandProcessor commandProcessor)
    {
        _commandProcessor = commandProcessor;
    }

    [HttpPut(CategoryConfigs.CategoriesPrefixUri, Name = "UpdateCategory")]
    [ProducesResponseType(typeof(StatusCodeResult), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(StatusCodeProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(StatusCodeProblemDetails), StatusCodes.Status400BadRequest)]
    [SwaggerOperation(
        Summary = "UpdateCategory",
        Description = "UpdateCategory",
        OperationId = "UpdateCategory",
        Tags = new[]
               {
                   CategoryConfigs.Tag
               })]
    [ApiVersion(1.0)]
    public override async Task HandleAsync(
        [FromBody] UpdateCategory request,
        CancellationToken cancellationToken = new CancellationToken()
    )
    {
        await _commandProcessor.SendAsync(request, cancellationToken);
    }
}
