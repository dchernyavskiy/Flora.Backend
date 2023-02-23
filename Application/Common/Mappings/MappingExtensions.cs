using AutoMapper;
using AutoMapper.QueryableExtensions;
using Flora.Application.Common.Models;
using Microsoft.EntityFrameworkCore;

namespace Flora.Application.Common.Mappings;

public static class MappingExtensions
{
    public static Task<PaginatedList<TDestination>> PaginatedListAsync<TDestination>(this IQueryable<TDestination> queryable, int pageNumber, int pageSize) where TDestination : class
        => PaginatedList<TDestination>.CreateAsync(queryable.AsNoTracking(), pageNumber, pageSize);
    
    public static Task<PaginatedList<TDestination>> PaginatedListAsync<TDestination>(this IEnumerable<TDestination> queryable, int pageNumber, int pageSize) where TDestination : class
        => PaginatedList<TDestination>.CreateAsync(queryable, pageNumber, pageSize);

    public static Task<List<TDestination>> ProjectToListAsync<TDestination>(this IQueryable queryable, IConfigurationProvider configuration) where TDestination : class
        => queryable.ProjectTo<TDestination>(configuration).AsNoTracking().ToListAsync();

    public static Task<Collection<TDestination>> ToCollectionAsync<TDestination>(
        this IQueryable<TDestination> queryable)
        => Collection<TDestination>.CreateAsync(queryable);
}
