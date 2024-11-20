/*using API.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigins",
        policy =>
        {
            policy.WithOrigins("http://localhost:4200")
                .AllowAnyMethod()
                .AllowAnyHeader();
        });
});

var app = builder.Build();

app.UseMiddleware<ApiKeyMiddleware>();

// Enable routing
app.UseRouting();

app.UseCors("AllowSpecificOrigins");

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();  // This maps the controller routes
});

app.Run();*/

using API.Middleware;
using Microsoft.EntityFrameworkCore;
using TadeoT.Database;
using TadeoT.Database.Functions;
using TadeoT.Database.Model;

var builder = WebApplication.CreateBuilder(args);
{
    builder.Services.AddDbContext<TadeoTDbContext>(options =>
        options.UseMySql(TadeoTDbContextFactory.GetConnectionString(),
            new MySqlServerVersion(new Version(8, 0, 32))));
    builder.Services.AddScoped<StopFunctions>();
    builder.Services.AddScoped<StopGroupFunctions>();
    builder.Services.AddScoped<DivisionFunctions>();
    builder.Services.AddScoped<APIKeyFunctions>();
    builder.Services.AddControllers();
}

var app = builder.Build();

app.UseRouting();

app.UseEndpoints(endpoints =>
{
    app.UseMiddleware<ApiKeyMiddleware>();
    endpoints.MapControllers();
});

app.Run();