using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using TadeoT.Database;

namespace TadeoT;

public class Program
{
    static void Main(string[] args)
    {
        using var context = new TadeoTDbContext();
        context.Database.EnsureCreated();

        // example program
        var stopGroup = new StopGroup
        {
            Name = "Main Building",
            Description = "Group for stops in the main building",
            Color = "#FF5733",
            Stops = []
        };

        var stop = new Stop
        {
            Name = "Room 101",
            Description = "Meeting room 101",
            RoomNr = "101",
            StopGroup = stopGroup,
            StopStatistics = []
        };

        context.StopGroups.Add(stopGroup);
        context.Stops.Add(stop);

        context.SaveChanges();

        var stopGroups = context.StopGroups.Include(sg => sg.Stops).ToList();
        foreach (var sg in stopGroups)
        {
            Console.WriteLine($"StopGroup: {sg.Name}, Color: {sg.Color}");
            foreach (var s in sg.Stops)
            {
                Console.WriteLine($"    Stop: {s.Name}, RoomNr: {s.RoomNr}");
            }
        }
    }
}
