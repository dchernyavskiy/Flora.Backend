using Ardalis.GuardClauses;
using AutoMapper;
using BuildingBlocks.Abstractions.CQRS.Queries;
using BuildingBlocks.Caching;
using BuildingBlocks.Core.Exception;
using Flora.Services.Catalogs.Shared.Extensions;
using Flora.Services.Catalogs.Products.Dtos.v1;
using Flora.Services.Catalogs.Products.Exceptions.Application;
using Flora.Services.Catalogs.Products.ValueObjects;
using Flora.Services.Catalogs.Shared.Contracts;
using FluentValidation;

namespace Flora.Services.Catalogs.Products.Features.GettingProductById.v1;

public record GetProductById(Guid Id) : IQuery<GetProductByIdResponse>
{
    internal class Validator : AbstractValidator<GetProductById>
    {
        public Validator()
        {
            CascadeMode = CascadeMode.Stop;

            RuleFor(x => x.Id);
        }
    }

    public class Cache : CacheRequest<GetProductById, GetProductByIdResponse>
    {
        public override string CacheKey(GetProductById request)
        {
            return $"{base.CacheKey(request)}_{request.Id}";
        }
    }

    internal class Handler : IQueryHandler<GetProductById, GetProductByIdResponse>
    {
        private readonly ICatalogDbContext _catalogDbContext;
        private readonly IMapper _mapper;

        public Handler(ICatalogDbContext catalogDbContext, IMapper mapper)
        {
            _catalogDbContext = catalogDbContext;
            _mapper = mapper;
        }

        public async Task<GetProductByIdResponse> Handle(GetProductById query, CancellationToken cancellationToken)
        {
            Guard.Against.Null(query, nameof(query));

            var product = await _catalogDbContext.FindProductByIdAsync(query.Id);
            Guard.Against.NotFound(product, new ProductNotFoundException(query.Id));

            var productsDto = _mapper.Map<ProductDto>(product);

            return new GetProductByIdResponse(productsDto);
        }
    }
}
