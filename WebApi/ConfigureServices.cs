using FluentValidation.AspNetCore;
using Flora.Application.Common.Interfaces;
using Flora.Infrastructure.Persistence;
using Flora.WebApi.Filters;
using Flora.WebApi.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Flora.WebApi;

public static class ConfigureServices
{
    public static IServiceCollection AddWebApiServices(this IServiceCollection services)
    {
        services.AddDatabaseDeveloperPageExceptionFilter();
        services.AddControllers(options =>
                options.Filters.Add<ApiExceptionFilterAttribute>())
            .AddNewtonsoftJson(opts =>
            {
                opts.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            })
            .AddFluentValidation(x => x.AutomaticValidationEnabled = false);
        services.AddVersionedApiExplorer(opts => opts.GroupNameFormat = "'v'VVV");
        services.AddSwaggerGen();
        services.AddHttpClient();
        services.AddApiVersioning();
        
        services.AddDataProtection();
        services.AddSingleton<IBasketService, BasketService>();
        
        services.AddSingleton<ICurrentUserService, CurrentUserService>();

        services.AddHttpContextAccessor();

        services.AddHealthChecks()
            .AddDbContextCheck<ApplicationDbContext>();

        services.Configure<ApiBehaviorOptions>(options =>
            options.SuppressModelStateInvalidFilter = true);
        
        services.AddTransient<IConfigureOptions<SwaggerGenOptions>, SwaggerConfigurationOptions>();
        
        return services;
    }

    public static IServiceCollection AddIdentityServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddCors(opts =>
        {
            opts.AddPolicy("AllowAll", policy =>
            {
                policy.AllowAnyHeader();
                policy.AllowAnyMethod();
                policy.AllowAnyOrigin();
            });
        });
        services.AddAuthentication(opts =>
        {
            opts.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            opts.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            opts.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            opts.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
            opts.DefaultSignOutScheme = JwtBearerDefaults.AuthenticationScheme;
            opts.DefaultForbidScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer("Bearer", opts =>
        {
            opts.Authority = configuration["Authority"];
            opts.Audience = "FloraWebApi";
            opts.RequireHttpsMetadata = false;
        });

        return services;
    }
}