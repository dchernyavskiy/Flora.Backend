using Ardalis.ApiEndpoints;
using Asp.Versioning;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using BuildingBlocks.Abstractions.CQRS.Queries;
using Flora.Services.Catalogs.Categories.Dtos;
using Flora.Services.Catalogs.Categories.Features.GettingCategories.v1;
using Flora.Services.Catalogs.Shared.Contracts;
using Hellang.Middleware.ProblemDetails;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;

namespace Flora.Services.Catalogs.Categories.Features.GettingCategory.v1;

public record GetCategory(Guid Id) : IQuery<GetCategoryResponse>;

public class GetCategoryHandler : IQueryHandler<GetCategory, GetCategoryResponse>
{
    private readonly ICatalogDbContext _context;
    private readonly IMapper _mapper;

    public GetCategoryHandler(ICatalogDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<GetCategoryResponse> Handle(GetCategory request, CancellationToken cancellationToken)
    {
        var categories = await _context.Categories.FromSqlRaw(
                                 $@"
WITH RECURSIVE category_cte AS
(SELECT id, name, description, image_url, parent_id FROM categories
WHERE id = @categoryId
UNION ALL
SELECT c.id, c.name, c.description, c.image_url, c.parent_id FROM categories c
WHERE c.parent_id = null
JOIN category_cte cte ON cte.id = c.parent_id)
SELECT c.id, c.name, c.description, c.image_url, c.parent_id,
ch.id, ch.name, ch.category_id FROM category_cte c
LEFT JOIN characteristics ch ON c.id = ch.category_id",
                                 new SqlParameter("categoryId", request.Id))
                             .Include(x => x.Characteristics)
                             .ProjectTo<BriefCategoryDto>(_mapper.ConfigurationProvider)
                             .ToListAsync(cancellationToken: cancellationToken);
        return new GetCategoryResponse();
    }
}

public record GetCategoryResponse();

public class GetCategoryEndpoint
    : EndpointBaseAsync.WithRequest<GetCategory>.WithResult<GetCategoryResponse>
{
    private readonly IQueryProcessor _queryProcessor;

    public GetCategoryEndpoint(IQueryProcessor queryProcessor)
    {
        _queryProcessor = queryProcessor;
    }

    [HttpGet(CategoryConfigs.CategoriesPrefixUri, Name = "GetCategory")]
    [ProducesResponseType(typeof(GetCategoryResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(StatusCodeProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(StatusCodeProblemDetails), StatusCodes.Status400BadRequest)]
    [ApiVersion(1.0)]
    [SwaggerOperation(
        Summary = "GetCategory",
        Description = "GetCategory",
        OperationId = "GetCategory",
        Tags = new[]
               {
                   CategoryConfigs.Tag
               })]
    public override async Task<GetCategoryResponse> HandleAsync(
        [FromQuery] GetCategory request,
        CancellationToken cancellationToken = new CancellationToken()
    )
    {
        return await _queryProcessor.SendAsync(request, cancellationToken);
    }
}
