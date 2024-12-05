using API.Endpoints;
using API.Middleware;
using Microsoft.EntityFrameworkCore;
using TadeoT.Database;
using TadeoT.Database.Functions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<TadeoTDbContext>(options =>
    options.UseMySql(TadeoTDbContextFactory.GetConnectionString(),
        new MySqlServerVersion(new Version(8, 0, 32))), ServiceLifetime.Transient);

builder.Services.AddScoped<DivisionFunctions>();
builder.Services.AddScoped<APIKeyFunctions>();
builder.Services.AddScoped<StopGroupFunctions>();
builder.Services.AddScoped<StopFunctions>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "default",
        policyBuilder =>
        {
            policyBuilder.WithOrigins("http://localhost:4200");
            policyBuilder.AllowAnyHeader();
            policyBuilder.AllowAnyMethod();
            policyBuilder.AllowCredentials();
        });
});

var app = builder.Build();

app.UseCors("default");

app.MapStopGroupEndpoints();
app.MapStopEndpoints();
app.MapDivisionEndpoints();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
        c.RoutePrefix = string.Empty;
    });
}

app.UseMiddleware<ApiKeyMiddleware>();

app.Run();