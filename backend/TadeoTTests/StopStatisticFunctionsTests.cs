using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TadeoT.Database.Functions;
using TadeoT.Database;
using Xunit.Priority;

namespace TadeoTTests;
public class StopStatisticFunctionsTests {
    private readonly TadeoTDbContext context = new();

    private readonly StopGroupFunctions stopGroupFunctions = new();
    private readonly StopFunctions stopFunctions = new();
    private readonly StopStatisticFunctions stopStatisticFunctions = new();

    private readonly StopStatistic testStatistic;
    private readonly StopGroup testGroup;
    private readonly Stop testStop;

    public StopStatisticFunctionsTests() {
        
        testGroup = new StopGroup() {
            StopGroupID = stopGroupFunctions.GetMaxId() + 1,
            Name = "TestGroup",
            Stops = [],
            Description = "TestDescription",
            Color = "#ffffff"
        };
        testStop = new Stop() {
            Name = "TestStop",
            Description = "TestDescription",
            RoomNr = "E72",
            StopID = this.stopFunctions.GetMaxId() + 1,
            StopGroup = testGroup,
            StopStatistics = []
        };
        testStatistic = new StopStatistic() {
            StopStatisticID = this.stopStatisticFunctions.GetMaxId() + 1,
            StopID = testStop.StopID,
            Stop = testStop,
            Time = DateTime.Now,
            IsDone = false
        };
    }

    [Fact, Priority(1)]
    public void GetStopStatisticByIdTest() {
        try {
            StopStatistic result = this.stopStatisticFunctions.GetStopStatisticById(1);
            Assert.Equal(1, result.StopStatisticID);
        } catch (Exception e) {
            Console.WriteLine(e.Message);
        }
        
    }

    [Fact, Priority(2)]
    public void AddStopStatisticTest() {
        try {
            this.stopStatisticFunctions.AddStopStatistic(this.testStatistic);
            StopStatistic result = this.stopStatisticFunctions.GetStopStatisticById(this.testGroup.StopGroupID);
            Assert.True(result != null);
        } catch(Exception e) {
            Console.WriteLine(e.Message);
        }
        
    }

    [Fact]
    public void UpdateStopStatisticTest() {
        try {
            StopStatistic stat = this.stopStatisticFunctions.GetStopStatisticById(this.testGroup.StopGroupID);
            stat.IsDone = true;
            this.stopStatisticFunctions.UpdateStopStatistic(stat);
            StopGroup result = this.stopGroupFunctions.GetStopGroupById(this.testGroup.StopGroupID);
            Assert.Equal("UpdatedName", result.Name);
        } catch (Exception e) {
            Console.WriteLine("Could not update Stop: " + e.Message);
        }
    }

    [Fact]
    public void DeleteStopStatistic() {
        try {
            StopStatistic stat = this.stopStatisticFunctions.GetStopStatisticById(this.testGroup.StopGroupID);
            this.stopGroupFunctions.DeleteStopGroupById(stat.StopStatisticID);
            Assert.Throws<Exception>(() => this.stopStatisticFunctions.GetStopStatisticById(testGroup.StopGroupID));
        } catch (Exception e) {
            Console.WriteLine("Could not delete StopGroup: " + e.Message);
        }
    }
}

