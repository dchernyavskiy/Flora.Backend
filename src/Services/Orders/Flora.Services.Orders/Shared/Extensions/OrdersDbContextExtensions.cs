using Microsoft.EntityFrameworkCore;
using Flora.Services.Orders.Orders.Models;
using Flora.Services.Orders.Orders.ValueObjects;
using Flora.Services.Orders.Shared.Data;

namespace Flora.Services.Orders.Shared.Extensions;

public static class OrdersDbContextExtensions
{
    public static ValueTask<Order?> FindOrderByIdAsync(this OrdersDbContext context, OrderId id)
    {
        return context.Orders.FindAsync(id);
    }

    public static Task<bool> ExistsOrderByIdAsync(this OrdersDbContext context, OrderId id)
    {
        return context.Orders.AnyAsync(x => x.Id == id);
    }
}
