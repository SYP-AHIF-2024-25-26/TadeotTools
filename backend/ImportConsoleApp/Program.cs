using Core.Entities;
using ImportConsoleApp;
using TadeoT.Database;
using TadeoT.Database.Functions;

Console.WriteLine("Recreate database ...");
using var context = new TadeoTDbContextFactory().CreateDbContext(args); 
await context.Database.EnsureDeletedAsync();
await context.Database.EnsureCreatedAsync();

Console.WriteLine("Generating a new api key ...");
var apiKeyFunctions = new APIKeyFunctions(context);
if ((await apiKeyFunctions.GetAllAPIKeys()).Count <= 0)
{
    await apiKeyFunctions.AddAPIKey(new APIKey { APIKeyValue = APIKeyGenerator.GenerateApiKey() });
}

Console.WriteLine("Importing data ...");

await CsvImporter.ImportCsvFileAsync("TdoT_Stationsplanung_2025.csv", context);

Console.WriteLine("Imported " + context.Stops.Count() + " stops");
Console.WriteLine("Imported " + context.StopGroups.Count() + " stop groups");
Console.WriteLine("Imported " + context.Divisions.Count() + " divisions");
Console.WriteLine("Imported " + context.StopGroupAssignments.Count() + " stop group assignments");

