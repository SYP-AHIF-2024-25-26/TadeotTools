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

// Enable routing
app.UseRouting();

app.UseCors("AllowSpecificOrigins");

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();  // This maps the controller routes
});

app.Run();