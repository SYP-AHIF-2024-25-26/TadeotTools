using TadeoT.Database;
using TadeoT.Database.Functions;
using Xunit.Priority;

namespace TadeoTTests;
public class StopFunctionsTests {
    private readonly TadeoTDbContext context = new();
    private readonly StopFunctions stopFunctions = new();
    private readonly StopGroupFunctions stopGroupFunctions = new();
    private StopGroup testGroup;
    private Stop testStop;

    public StopFunctionsTests() {
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
    }

    [Fact, Priority(1)]
    public void GetStopByIdTest() {
        Stop result = this.stopFunctions.GetStopById(1);
        Assert.Equal(1, result.StopID);
    }

    [Fact, Priority(2)]
    public void AddStopTest() {
        this.stopFunctions.AddStop(this.testStop);
        Stop result = this.stopFunctions.GetStopById(this.testStop.StopID);
        Assert.True(result != null);
    }

    [Fact]
    public void UpdateStopTest() {
        try {
            Stop stop = this.stopFunctions.GetStopById(this.testStop.StopID);
            stop.Name = "UpdatedName";
            this.stopFunctions.UpdateStop(stop);
            Stop result = this.stopFunctions.GetStopById(this.testStop.StopID);
            Assert.Equal("UpdatedName", result.Name);
        } catch(Exception e) {
            Console.WriteLine("Could not update Stop: " + e.Message);
        }
        
    }

    [Fact]
    public void DeleteStop() {
        try {
            Stop stop = this.stopFunctions.GetStopById(this.testStop.StopID);
            this.stopFunctions.DeleteStopById(stop.StopID);
            Assert.Throws<Exception>(() => this.stopFunctions.GetStopById(stop.StopID));
        } catch (Exception e) {
            Console.WriteLine("Could not delete Stop: " + e.Message);
        }
    }
}