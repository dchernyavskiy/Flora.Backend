using IdentityServer4.Services;
using Flora.Identity.Data;
using Flora.Identity.Interfaces;
using Flora.Identity.Models;
using Flora.Identity.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Flora.Identity;

internal class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddDbContext<AuthDbContext>(opts =>
        {
            opts.UseSqlServer(builder.Configuration["DbConnection"]!);
        });

        builder.Services.AddIdentity<AppUser, IdentityRole>(opts =>
            {
                opts.Password.RequiredLength = 4;
                opts.Password.RequireDigit = true;
                opts.Password.RequireNonAlphanumeric = false;
                opts.Password.RequireLowercase = false;
                opts.Password.RequireUppercase = false;
                opts.User.RequireUniqueEmail = true;
            })
            .AddEntityFrameworkStores<AuthDbContext>()
            .AddDefaultTokenProviders();

        builder.Services.AddIdentityServer()
            .AddAspNetIdentity<AppUser>()
            .AddInMemoryApiResources(IdentityConfiguration.ApiResources)
            .AddInMemoryIdentityResources(IdentityConfiguration.IdentityResources)
            .AddInMemoryApiScopes(IdentityConfiguration.ApiScopes)
            .AddInMemoryClients(IdentityConfiguration.Clients)
            .AddDeveloperSigningCredential();

        builder.Services.ConfigureApplicationCookie(opts =>
        {
            opts.Cookie.Name = "Flora.Identity.Cookie";
            opts.LoginPath = "/Auth/Login";
            opts.LogoutPath = "/Auth/Logout";
        });

        builder.Services.AddControllersWithViews();

        const string allowWebApp = "AllowWebApp";
        builder.Services.AddCors(opts =>
        {
            opts.AddPolicy(allowWebApp, policy =>
            {
                policy.WithOrigins(builder.Configuration["WebApplicationUri"]!)
                    .AllowCredentials()
                    .AllowAnyHeader()
                    .AllowAnyMethod();
            });
        });

        builder.Services.AddScoped<AuthDbContextInitializer>();
        builder.Services.AddTransient<IProfileService, ProfileService>();
        builder.Services.AddScoped<IEmailSender, EmailSender>();

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            using (var scope = app.Services.CreateScope())
            {
                var initializer = scope.ServiceProvider.GetService<AuthDbContextInitializer>()!;
                await initializer.SeedAsync();
            }
        }

        app.UseCors(allowWebApp);
        app.UseStaticFiles();
        app.UseIdentityServer();
        app.MapControllers();

        app.Run();
    }
}