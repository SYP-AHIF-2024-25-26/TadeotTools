using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using TadeoT.Database;
using TadeoT.Database.Functions;

namespace TadeoT;

public class Program {
    static void Main(string[] args) {
        TadeoTDbContext context = new();
        context.Database.EnsureCreated();

        // example program
        StopGroup stopGroup = new() {
            Name = "Main Building",
            Description = "Group for stops in the main building",
            Color = "#FF5733",
            Stops = []
        };

        Stop stop = new() {
            Name = "Room 101",
            Description = "Meeting room 101",
            RoomNr = "101",
            StopGroup = stopGroup,
            StopStatistics = []
        };

        context.StopGroups.Add(stopGroup);
        context.Stops.Add(stop);

        context.SaveChanges();

        List<StopGroup> stopGroups = StopGroupFunctions.GetInstance().GetAllStopGroups();
        foreach (StopGroup sg in stopGroups) {
            Console.WriteLine($"StopGroup: {sg.Name}, Color: {sg.Color}");
            foreach (Stop s in sg.Stops) {
                Console.WriteLine($"    Stop: {s.Name}, RoomNr: {s.RoomNr}");
            }
        }
        Console.WriteLine("Stops: ");
        List<Stop> stops = StopFunctions.GetInstance().GetAllStops();
        foreach (Stop s in stops) {
            Console.WriteLine($"Stop: {s.Name}, RoomNr: {s.RoomNr}");
        }

    }
}
