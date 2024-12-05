using API.Endpoints;
using API.Middleware;
using Microsoft.EntityFrameworkCore;
using TadeoT.Database;
using TadeoT.Database.Functions;

/*var builder = WebApplication.CreateBuilder(args);
{
    builder.Services.AddDbContext<TadeoTDbContext>(options =>
        options.UseMySql(TadeoTDbContextFactory.GetConnectionString(),
            new MySqlServerVersion(new Version(8, 0, 32))), ServiceLifetime.Transient);

    builder.Services.AddScoped<StopFunctions>();
    builder.Services.AddScoped<StopGroupFunctions>();
    builder.Services.AddScoped<DivisionFunctions>();
    builder.Services.AddScoped<APIKeyFunctions>();
    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
}

builder.Services.AddCors(options =>  
{  
    options.AddPolicy(name: "default",  
        policy  =>  
        {  
            policy.WithOrigins("http://localhost:4200"); // add the allowed origins  
        });  
});  

var app = builder.Build();

app.UseEndpoints(endpoints => endpoints.MapControllers());

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
        c.RoutePrefix = string.Empty; // Sets Swagger UI at root
    });
}

app.UseMiddleware<ApiKeyMiddleware>();
app.UseRouting();
app.UseCors("default");


app.Run();*/

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
            policyBuilder.WithOrigins("http://localhost:4200", "http://localhost:4300");
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
        c.RoutePrefix = string.Empty; // Sets Swagger UI at root
    });
}

//app.UseMiddleware<ApiKeyMiddleware>();  // Add the ApiKeyMiddleware here

app.Run();