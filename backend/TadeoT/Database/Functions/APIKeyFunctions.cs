using TadeoT.Database.Model;

namespace TadeoT.Database.Functions;

public class APIKeyFunctions {
    private readonly TadeoTDbContext context = new();
    private static APIKeyFunctions? instance;

    private APIKeyFunctions() { }

    public static APIKeyFunctions GetInstance() {
        instance ??= new APIKeyFunctions();
        return instance;
    }

    public List<APIKey> GetAllAPIKeys() {
        return [.. this.context.APIKeys];
    }

    public APIKey AddAPIKey(APIKey apiKey) {
        try {
            this.context.APIKeys.Add(apiKey);
            this.context.SaveChanges();
            return apiKey;
        } catch (Exception e) {
            throw new TadeoTDatabaseException("Could not add APIKey: " + e.Message);
        }
    }

    public void DeleteAPIKey(APIKey apiKey) {
        try {
            this.context.APIKeys.Remove(apiKey);
            this.context.SaveChanges();
        } catch (Exception e) {
            throw new TadeoTDatabaseException("Could not delete APIKey: " + e.Message);
        }
    }

    public APIKey GetLastAPIKey() {
        return this.context.APIKeys.Last();
    }
}
