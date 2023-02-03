using Flora.Application.Common.Interfaces;
using Flora.Infrastructure.Persistence;
using Flora.Infrastructure.Persistence.Interceptors;
using Flora.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Flora.Infrastructure;

public static class ConfigureServices
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<AuditableEntitySaveChangesInterceptor>();

        // if (configuration.GetValue<bool>("UseInMemoryDatabase"))
        // {
        //     services.AddDbContext<ApplicationDbContext>(options =>
        //         options.UseInMemoryDatabase("FloraDb"));
        // }
        // else
        // {
        //     services.AddDbContext<ApplicationDbContext>(options =>
        //         options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"),
        //             builder => builder.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));
        // }
        //
        // services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());
        services.AddDbContext<IApplicationDbContext, ApplicationDbContext>(opts =>
        {
            opts.UseSqlServer(configuration["DbConnection"],
            builder => builder.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName));
        });
        services.AddScoped<ApplicationDbContextInitializer>();

        services.AddTransient<IDateTime, DateTimeService>();

        // services.AddAuthentication()
        //     .AddIdentityServerJwt();
        //
        // services.AddAuthorization(options =>
        //     options.AddPolicy("CanPurge", policy => policy.RequireRole("Administrator")));

        return services;
    }
}
