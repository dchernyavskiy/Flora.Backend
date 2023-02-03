using Flora.Application;
using Flora.Infrastructure;
using Flora.Infrastructure.Persistence;
using Flora.WebApi;
using Minio.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddWebApiServices();
builder.Services.AddIdentityServices(builder.Configuration);

builder.Services.AddMinio(opts =>
{
    opts.Endpoint = builder.Configuration["Minio:Endpoint"]!;
    opts.AccessKey = builder.Configuration["Minio:AccessKey"]!;
    opts.SecretKey = builder.Configuration["Minio:SecretKey"]!;
    opts.ConfigureClient(client =>
    {
        client.Build();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    using (var scope = app.Services.CreateScope())
    {
        var initializer = scope.ServiceProvider.GetService<ApplicationDbContextInitializer>();
        await initializer?.SeedAsync()!;
    }
}

app.UseCors("AllowAll");
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.UseApiVersioning();
app.MapControllers();

app.Run();