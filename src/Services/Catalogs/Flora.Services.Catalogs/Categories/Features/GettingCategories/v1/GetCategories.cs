using Ardalis.ApiEndpoints;
using Asp.Versioning;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using BuildingBlocks.Abstractions.CQRS.Queries;
using Flora.Services.Catalogs.Categories.Dtos;
using Flora.Services.Catalogs.Shared.Contracts;
using Hellang.Middleware.ProblemDetails;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;

namespace Flora.Services.Catalogs.Categories.Features.GettingCategories.v1;

public record GetCategories() : IQuery<GetCategoriesResponse>;

public class GetCategoriesHandler : IQueryHandler<GetCategories, GetCategoriesResponse>
{
    private readonly ICatalogDbContext _context;
    private readonly IMapper _mapper;

    public GetCategoriesHandler(ICatalogDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<GetCategoriesResponse> Handle(GetCategories request, CancellationToken cancellationToken)
    {
        var categories = await _context.Categories.FromSql(
                                 $@"
WITH recursive category_cte AS
(SELECT id, name, description, image_url, parent_id FROM catalog.categories
UNION ALL
SELECT c.id, c.name, c.description, c.image_url, c.parent_id FROM catalog.categories c
JOIN category_cte cte on cte.id = c.parent_id
WHERE c.parent_id is null)
SELECT * FROM category_cte cte")
                             .Include(x => x.Characteristics)
                             .ProjectTo<BriefCategoryDto>(_mapper.ConfigurationProvider)
                             .ToListAsync(cancellationToken: cancellationToken);
        return new GetCategoriesResponse(categories);
    }
}

public record GetCategoriesResponse(ICollection<BriefCategoryDto> Categories);

public class GetCategoriesEndpoint
    : EndpointBaseAsync.WithRequest<GetCategories>.WithResult<GetCategoriesResponse>
{
    private readonly IQueryProcessor _queryProcessor;

    public GetCategoriesEndpoint(IQueryProcessor queryProcessor)
    {
        _queryProcessor = queryProcessor;
    }

    [HttpGet(CategoryConfigs.CategoriesPrefixUri + "/get-all", Name = "GetCategories")]
    [ProducesResponseType(typeof(GetCategoriesResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(StatusCodeProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(StatusCodeProblemDetails), StatusCodes.Status400BadRequest)]
    [ApiVersion(1.0)]
    [SwaggerOperation(
        Summary = "GetCategories",
        Description = "GetCategories",
        OperationId = "GetCategories",
        Tags = new[]
               {
                   CategoryConfigs.Tag
               })]
    public override async Task<GetCategoriesResponse> HandleAsync(
        [FromQuery] GetCategories request,
        CancellationToken cancellationToken = new CancellationToken()
    )
    {
        return await _queryProcessor.SendAsync(request, cancellationToken);
    }
}
