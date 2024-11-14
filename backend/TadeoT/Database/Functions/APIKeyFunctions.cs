using Microsoft.EntityFrameworkCore;
using TadeoT.Database.Model;

namespace TadeoT.Database.Functions;

public class APIKeyFunctions {
    private readonly TadeoTDbContext context;

    public APIKeyFunctions(TadeoTDbContext context) {
        this.context = context;
    }

    public async Task<List<APIKey>> GetAllAPIKeys() {
        try {
            return await this.context.APIKeys.ToListAsync();
        } catch (Exception e) {
            throw new TadeoTDatabaseException("Could not retrieve APIKeys: " + e.Message);
        }
    }

    public async Task<APIKey> AddAPIKey(APIKey apiKey) {
        try {
            this.context.APIKeys.Add(apiKey);
            await this.context.SaveChangesAsync();
            return apiKey;
        } catch (Exception e) {
            throw new TadeoTDatabaseException("Could not add APIKey: " + e.Message);
        }
    }

    public async void DeleteAPIKey(APIKey apiKey) {
        try {
            this.context.APIKeys.Remove(apiKey);
            await this.context.SaveChangesAsync();
        } catch (Exception e) {
            throw new TadeoTDatabaseException("Could not delete APIKey: " + e.Message);
        }
    }

    public async Task<APIKey> GetLastAPIKey() {
        return await this.context.APIKeys.LastAsync();
    }
}
