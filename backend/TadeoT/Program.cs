using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using TadeoT.Database;

namespace TadeoT;

public class Program {
    static void Main(string[] args) {
        TadeoTDbContext context = new();
        context.Database.EnsureCreated();

        // example program
        StopGroup stopGroup = new() {
            Name = "Main Building",
            Description = "Group for stops in the main building",
            Color = "#FF5733"
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

        List<StopGroup> stopGroups = context.StopGroups.Include(sg => sg.Stops).ToList();
        foreach (StopGroup sg in stopGroups) {
            Console.WriteLine($"StopGroup: {sg.Name}, Color: {sg.Color}");
            foreach (Stop s in StopGroupFunctions.GetInstance().GetStopsOfStopGroup(sg.StopGroupID)) {
                Console.WriteLine($"    Stop: {s.Name}, RoomNr: {s.RoomNr}");
            }
        }
    }
}
