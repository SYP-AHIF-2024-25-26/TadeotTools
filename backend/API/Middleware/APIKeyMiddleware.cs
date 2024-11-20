using System.Security.Cryptography;
using System.Text;
using TadeoT.Database;
using TadeoT.Database.Functions;

namespace API.Middleware;

public class ApiKeyMiddleware
{
    private const string ApiKeyHeaderName = "X-Api-Key";
    private readonly RequestDelegate _next;
    private APIKeyFunctions _apiKeyRepository;

    public ApiKeyMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        this._apiKeyRepository = context.RequestServices.GetRequiredService<APIKeyFunctions>();
        // Only apply to api routes
        if (context.Request.Path.ToString().Contains("api"))
        {
            if (!context.Request.Headers.TryGetValue(ApiKeyHeaderName, out var extractedApiKey))
            {
                context.Response.StatusCode = 401; // Unauthorized
                await context.Response.WriteAsync("API Key was not provided.");
                return;
            }

            if (! await IsValidApiKey(extractedApiKey!))
            {
                context.Response.StatusCode = 403; // Forbidden
                await context.Response.WriteAsync("Invalid API Key.");
                return;
            }
        }

        await _next(context);
    }

    private async Task<bool> IsValidApiKey(string userApiKey)
    {
        List<string> systemApiKeys = (await _apiKeyRepository.GetAllAPIKeys()).Select(ak => ak.APIKeyValue).ToList();

        byte[] userApiKeyBytes = Encoding.UTF8.GetBytes(userApiKey);

        bool result = false;

        systemApiKeys.ForEach(systemApiKey =>
        {
            byte[] systemApiKeyBytes = Encoding.UTF8.GetBytes(systemApiKey);

            if (CryptographicOperations.FixedTimeEquals(userApiKeyBytes, systemApiKeyBytes))
            {
                result = true;
            }
        });
        return result;
    }
}