using Microsoft.EntityFrameworkCore;

namespace Flora.Application.Common.Models;

public class Collection<T>
{
    public List<T> Items { get; set; }
    public int Count { get; set; }

    public static async Task<Collection<T>> CreateAsync<T>(IQueryable<T> queryable)
    {
        var count = await queryable.CountAsync();
        return new Collection<T>
        {
            Items  = await queryable.ToListAsync(),
            Count = count
        };
    }
}