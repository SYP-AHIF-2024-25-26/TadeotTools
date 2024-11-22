using API.Middleware;
using Microsoft.EntityFrameworkCore;
using TadeoT.Database;
using TadeoT.Database.Functions;

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
    builder.Services.AddCors(options =>
    {
        options.AddPolicy("AllowAll", policy =>
        {
            policy.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader();
        });
    });
}

var app = builder.Build();

app.UseRouting();

app.UseCors("AllowAll");

app.UseEndpoints(endpoints =>
{
    app.UseMiddleware<ApiKeyMiddleware>();
    endpoints.MapControllers();
});

app.Run();