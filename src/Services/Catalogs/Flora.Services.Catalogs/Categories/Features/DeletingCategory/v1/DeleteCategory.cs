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

namespace Flora.Services.Catalogs.Categories.Features.DeletingCategory.v1;

public record DeleteCategory(Guid Id) : IDeleteCommand<Guid>;

public class DeleteCategoryHandler : ICommandHandler<DeleteCategory>
{
    private readonly ICatalogDbContext _context;

    public DeleteCategoryHandler(ICatalogDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(DeleteCategory request, CancellationToken cancellationToken)
    {
        var category = await _context.Categories.FirstOrDefaultAsync(
                           x => x.Id == request.Id,
                           cancellationToken: cancellationToken);

        Guard.Against.Null(category);

        _context.Categories.Remove(category);
        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}

public class DeleteCategoryEndpoint : EndpointBaseAsync.WithRequest<DeleteCategory>.WithoutResult
{
    private readonly ICommandProcessor _commandProcessor;

    public DeleteCategoryEndpoint(ICommandProcessor commandProcessor)
    {
        _commandProcessor = commandProcessor;
    }

    [HttpDelete(CategoryConfigs.CategoriesPrefixUri, Name = "DeleteCategory")]
    [ProducesResponseType(typeof(StatusCodeResult), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(StatusCodeProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(StatusCodeProblemDetails), StatusCodes.Status400BadRequest)]
    [SwaggerOperation(
        Summary = "DeleteCategory",
        Description = "DeleteCategory",
        OperationId = "DeleteCategory",
        Tags = new[]
               {
                   CategoryConfigs.Tag
               })]
    [ApiVersion(1.0)]
    public override async Task HandleAsync(
        [FromBody] DeleteCategory request,
        CancellationToken cancellationToken = new CancellationToken()
    )
    {
        await _commandProcessor.SendAsync(request, cancellationToken);
    }
}
