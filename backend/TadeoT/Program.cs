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
    public IConfiguration Configuration { get; }

    public Program(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public static void InitializeDatabase()
    {
        DotNetEnv.Env.Load();

        using (var context = new TadeoTDbContext())
        {
            context.Database.EnsureCreated();

            if (!context.APIKeys.Any())
            {
                context.APIKeys.Add(new APIKey { APIKeyValue = APIKeyGenerator.GenerateApiKey() });
                context.SaveChanges();
            }
        }
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddDbContext<TadeoTDbContext>((serviceProvider, optionsBuilder) =>
        {
            var serverName = Configuration["Database:ServerName"];
            var serverPort = Configuration["Database:ServerPort"];
            var databaseName = Configuration["Database:DatabaseName"];
            var username = Configuration["Database:Username"];
            var password = Configuration["Database:Password"];

            var connectionString = $"Server={serverName};Port={serverPort};Database={databaseName};User={username};Password={password};";
            optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
        }, ServiceLifetime.Scoped);

        services.AddScoped<StopGroupFunctions>();
        services.AddScoped<StopFunctions>();
        services.AddScoped<DivisionFunctions>();
        services.AddScoped<APIKeyFunctions>();
        services.AddScoped<StopStatisticFunctions>();
    }
    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureServices((context, services) =>
            {
                var program = new Program(context.Configuration);
                program.ConfigureServices(services);
            });

    static void Main(string[] args)
    {
        InitializeDatabase();
        /*
        var host = CreateHostBuilder(args).Build();

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
