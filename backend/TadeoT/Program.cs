using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Microsoft.EntityFrameworkCore;
using TadeoT.Database;
using TadeoT.Database.Functions;
using TadeoT.Database.Model;

namespace TadeoT;

public class Program {
    static void Main(string[] args) {
        TadeoTDbContext context = new();
        context.Database.EnsureCreated();

        if (!context.APIKeys.Any()) {
            context.APIKeys.Add(new APIKey { APIKeyValue = APIKeyGenerator.GenerateApiKey() });
            context.SaveChanges();
        }

        List<StopGroup> stopGroups = StopGroupFunctions.GetInstance().GetAllStopGroups();
        foreach (StopGroup sg in stopGroups) {
            Console.WriteLine($"StopGroup: {sg.Name}");
            foreach (Stop s in StopGroupFunctions.GetInstance().GetStopsOfStopGroup(sg.StopGroupID)) {
                Console.WriteLine($"    Stop: {s.Name}, RoomNr: {s.RoomNr}");
            }
        }
        Console.WriteLine("Stops: ");
        List<Stop> stops = StopFunctions.GetInstance().GetAllStops();
        foreach (Stop s in stops) {
            Console.WriteLine($"Stop: {s.Name}, RoomNr: {s.RoomNr}");
        }

        List<APIKey> keys = APIKeyFunctions.GetInstance().GetAllAPIKeys();
        foreach (APIKey k in keys) {
            Console.WriteLine($"APIKey: {k.APIKeyValue}");
        }

        Console.WriteLine("Divisions");
        List<Division> divisions = DivisionFunctions.GetInstance().GetAllDivisions();
        foreach (Division d in divisions) {
            Console.WriteLine($"Division: {d.Name}, Color: {d.Color}");
            foreach (Stop s in DivisionFunctions.GetInstance().GetStopsOfDivisionId(d.DivisionID)) {
                Console.WriteLine($"    Stop: {s.Name}, RoomNr: {s.RoomNr}");
            }
        }
    }
}
