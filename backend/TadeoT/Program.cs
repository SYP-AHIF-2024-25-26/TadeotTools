using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Microsoft.EntityFrameworkCore;
using TadeoT.Database;
using TadeoT.Database.Functions;
using TadeoT.Database.Model;

namespace TadeoT;

public class Program
{
    private static ServiceProvider? ServiceProvider = null;

    private static ServiceProvider BuildServiceProvider()
    {
        var config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();

        var services = new ServiceCollection();

        services.AddDbContext<TadeoTDbContext>(options =>
            options.UseMySql(
                TadeoTDbContextFactory.GetConnectionString(),
                ServerVersion.AutoDetect(TadeoTDbContextFactory.GetConnectionString())
            ));
        services.AddScoped<APIKeyFunctions>();

        return services.BuildServiceProvider();
    }


    public static async void InitializeDatabase()
    {
        DotNetEnv.Env.Load();

        ServiceProvider ??= BuildServiceProvider();

        using var scope = ServiceProvider!.CreateScope();
        var apiKeyFunctions = scope.ServiceProvider.GetRequiredService<APIKeyFunctions>();


        if ((await apiKeyFunctions.GetAllAPIKeys()).Count <= 0)
        {
            await apiKeyFunctions.AddAPIKey(new APIKey { APIKeyValue = APIKeyGenerator.GenerateApiKey() });
        }
    }

    static void Main(string[] args)
    {
        InitializeDatabase();
        /*

        var stopGroupFunctions = host.Services.GetRequiredService<StopGroupFunctions>();
        var stopFunctions = host.Services.GetRequiredService<StopFunctions>();
        var divisionFunctions = host.Services.GetRequiredService<DivisionFunctions>();
        var apiKeyFunctions = host.Services.GetRequiredService<APIKeyFunctions>();

        List<StopGroup> stopGroups = await stopGroupFunctions.GetAllStopGroups();
        foreach (StopGroup sg in stopGroups) {
            Console.WriteLine($"StopGroup: {sg.Name}");
            foreach (Stop s in await stopGroupFunctions.GetStopsOfStopGroup(sg.StopGroupID)) {
                Console.WriteLine($"\tStop: {s.Name}, RoomNr: {s.RoomNr}");
            }
        }
        Console.WriteLine("Stops: ");
        List<Stop> stops = await stopFunctions.GetAllStops();
        foreach (Stop s in stops) {
            Console.WriteLine($"Stop: {s.Name}, RoomNr: {s.RoomNr}");
        }

        List<APIKey> keys = await apiKeyFunctions.GetAllAPIKeys();
        foreach (APIKey k in keys) {
            Console.WriteLine($"APIKey: {k.APIKeyValue}");
        }

        Console.WriteLine("Divisions");
        List<Division> divisions = await divisionFunctions.GetAllDivisions();
        foreach (Division d in divisions) {
            Console.WriteLine($"Division: {d.Name}, Color: {d.Color}");
            foreach (Stop s in await divisionFunctions.GetStopsOfDivisionId(d.DivisionID)) {
                Console.WriteLine($"\tStop: {s.Name}, RoomNr: {s.RoomNr}");
            }
        }*/
    }
}
