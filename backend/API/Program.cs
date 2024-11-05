var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

var app = builder.Build();

// Enable routing
app.UseRouting();

// Enable CORS (if needed)
app.UseCors(x => x
    .AllowAnyMethod()
    .AllowAnyHeader()
    .SetIsOriginAllowed(origin => true)
    .AllowCredentials());

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();  // This maps the controller routes
});

app.Run();