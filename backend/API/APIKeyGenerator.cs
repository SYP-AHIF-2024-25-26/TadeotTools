using System.Security.Cryptography;
using System.Text;

namespace API;

public static class APIKeyGenerator
{
    public static string GenerateApiKey(int length = 32)
    {
        using var cryptoProvider = new RNGCryptoServiceProvider();
        byte[] apiKeyBytes = new byte[length];
        cryptoProvider.GetBytes(apiKeyBytes);
        
        StringBuilder apiKey = new StringBuilder(length * 2);
        foreach (byte b in apiKeyBytes)
        {
            apiKey.Append(b.ToString("x2")); // Converts to hexadecimal string
        }

        Console.WriteLine(apiKey.ToString());
        return apiKey.ToString();
    }
}