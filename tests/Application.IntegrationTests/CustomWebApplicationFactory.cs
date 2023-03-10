using Flora.Application.Common.Interfaces;
using Flora.Infrastructure.Persistence;
using Flora.WebApi;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace Flora.Application.IntegrationTests;

internal class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureAppConfiguration(configurationBuilder =>
        {
            var integrationConfig = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddEnvironmentVariables()
                .Build();

            configurationBuilder.AddConfiguration(integrationConfig);
        });

        builder.ConfigureServices((builder, services) =>
        {
            services
                .Remove<ICurrentUserService>()
                .AddSingleton<ICurrentUserService>(provider =>
                {
                    var mockService = new Mock<ICurrentUserService>();
                    mockService.Setup(x => x.UserId).Returns(UserIdProvider.Id);
                    return mockService.Object;
                });

            services
                .AddSingleton<IHttpContextAccessor>(provider =>
                {
                    var accessor = new Mock<IHttpContextAccessor>();
                    accessor.Setup(x => x.HttpContext).Returns(() => new DefaultHttpContext());
                    return accessor.Object;
                });
            
            services
                .Remove<DbContextOptions<ApplicationDbContext>>()
                .AddDbContext<ApplicationDbContext>((sp, options) =>
                    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"),
                        builder => builder.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName))
                );
        });

        builder.UseEnvironment("Development");
    }
}

public class UserIdProvider
{
    public static Guid Id { get; set; } = Guid.NewGuid();
}