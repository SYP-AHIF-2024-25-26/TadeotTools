namespace API.Middleware;

public static class MiddlewareExtensions
{
    public static WebApplication DefineApiKeyMiddleware(this WebApplication app)
    {
        app.Use(async (context, next) =>
        {
            Console.WriteLine($"Request Path: {context.Request.Path}");
            await next();
            Console.WriteLine($"Response Status Code: {context.Response.StatusCode}");
        });
        return app;
    }
}
