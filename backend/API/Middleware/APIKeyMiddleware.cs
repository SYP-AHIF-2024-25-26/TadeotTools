using System.Security.Cryptography;
using System.Text;
using TadeoT.Database;
using TadeoT.Database.Functions;

namespace API.Middleware;

public class ApiKeyMiddleware
{
    private readonly RequestDelegate next;
    private const string ApiKeyHeaderName = "X-Api-Key";

    public ApiKeyMiddleware(RequestDelegate next)
    {
        this.next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // Only apply to api routes
        if (context.Request.Path.ToString().Contains("/api"))
        {
            if (!context.Request.Headers.TryGetValue(ApiKeyHeaderName, out var extractedApiKey))
            {
                context.Response.StatusCode = 401; // Unauthorized
                await context.Response.WriteAsync("API Key was not provided.");
                return;
            }

            if (!IsValidApiKey(extractedApiKey!))
            {
                context.Response.StatusCode = 403; // Forbidden
                await context.Response.WriteAsync("Invalid API Key.");
                return;
            }
        }

        await this.next(context);
    }

    private static bool IsValidApiKey(string userApiKey)
    {
        List<string> systemApiKeys = APIKeyFunctions.GetInstance().GetAllAPIKeys().Select(ak => ak.APIKeyValue).ToList();
        
        byte[] userApiKeyBytes = Encoding.UTF8.GetBytes(userApiKey);
        
        bool result = false;
        
        systemApiKeys.ForEach(systemApiKey => {
            byte[] systemApiKeyBytes = Encoding.UTF8.GetBytes(systemApiKey);

            if (CryptographicOperations.FixedTimeEquals(userApiKeyBytes, systemApiKeyBytes))
            {
                result = true;
            }
        });
        return result;
    }
}
