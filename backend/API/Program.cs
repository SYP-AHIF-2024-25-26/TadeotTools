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
        options.AddPolicy(name: "default",
            policy  =>
            {
                policy.WithOrigins("http://localhost:4200")
                    .AllowAnyHeader()
                    .AllowAnyMethod();
            });
    });
    
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
}

var app = builder.Build();

app.UseCors("default"); // Place before UseRouting
app.UseRouting();

app.UseMiddleware<ApiKeyMiddleware>(); // Place after UseCors
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
        c.RoutePrefix = string.Empty; // Sets Swagger UI at root
    });
}

app.Run();
