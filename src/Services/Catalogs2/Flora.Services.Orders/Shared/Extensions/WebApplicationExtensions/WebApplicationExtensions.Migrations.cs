using Flora.Services.Orders.Shared.Data;
using Microsoft.EntityFrameworkCore;

namespace Flora.Services.Orders.Shared.Extensions.WebApplicationExtensions;

public static partial class WebApplicationExtensions
{
    public static async Task ApplyDatabaseMigrations(this WebApplication app)
    {
        var configuration = app.Services.GetRequiredService<IConfiguration>();
        if (configuration.GetValue<bool>("PostgresOptions:UseInMemory") == false)
        {
            using var serviceScope = app.Services.CreateScope();
            var catalogDbContext = serviceScope.ServiceProvider.GetRequiredService<OrderDbContext>();

            app.Logger.LogInformation("Updating catalog database...");

            await catalogDbContext.Database.MigrateAsync();

            app.Logger.LogInformation("Updated catalog database");
        }
    }
}
