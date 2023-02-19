using Microsoft.EntityFrameworkCore;

namespace Flora.Application.Common.Models;

public class Collection<T>
{
    public List<T> Items { get; set; }
    public int Count { get; set; }

    public static Task<Collection<T>> CreateAsync<T>(IQueryable<T> queryable)
    {
        return Task.FromResult(new Collection<T>()
        {
            Items = queryable.ToList(),
            Count = queryable.Count()
        });
    }
}