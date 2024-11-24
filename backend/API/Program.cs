using API.Middleware;
using Microsoft.EntityFrameworkCore;
using TadeoT.Database;
using TadeoT.Database.Functions;

var builder = WebApplication.CreateBuilder(args);
{
    builder.Services.AddDbContext<TadeoTDbContext>(options =>
        options.UseMySql(TadeoTDbContextFactory.GetConnectionString(),
            new MySqlServerVersion(new Version(8, 0, 32))), ServiceLifetime.Transient);
    
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
    
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
}

var app = builder.Build();

app.UseRouting();

app.UseCors("AllowAll");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
        c.RoutePrefix = string.Empty; // Sets Swagger UI at root
    });
}

app.UseEndpoints(endpoints =>
{
    app.UseMiddleware<ApiKeyMiddleware>();
    endpoints.MapControllers();
});

app.Run();