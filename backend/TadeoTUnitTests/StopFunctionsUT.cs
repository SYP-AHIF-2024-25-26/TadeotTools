using TadeoT.Database;
using TadeoT.Database.Functions;


namespace TadeoTUnitTests;

public class StopFunctionsUT {
    private readonly TadeoTDbContext context = new();
    private readonly StopFunctions stopFunctions = new();
    private readonly StopGroupFunctions stopGroupFunctions = new();

    private readonly StopGroup testGroup;
    private readonly Stop testStop;

    //private readonly GlobalSetup setup = new GlobalSetup();

    public StopFunctionsUT() {
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

    [Test, Order(1)]
    public void AddStopTest() {
        this.stopGroupFunctions.AddStopGroup(this.testGroup);
        StopGroup resultG = this.stopGroupFunctions.GetStopGroupById(this.testGroup.StopGroupID);
        Assert.That(resultG, Is.Not.EqualTo(null));
    }

    [Test]
    public void GetStopByIdTest() {
        Stop result = this.stopFunctions.GetStopById(1);
        Assert.That(result.StopID, Is.EqualTo(1));
    }

    [Test]
    public void UpdateStopTest() {
        Stop stop = this.stopFunctions.GetStopById(this.testStop.StopID);
        stop.Name = "UpdatedName";
        this.stopFunctions.UpdateStop(stop);
        Stop result = this.stopFunctions.GetStopById(this.testStop.StopID);
        Assert.That(result.Name, Is.EqualTo("UpdatedName"));
    }

    [Test]
    public void DeleteStop() {
        Stop stop = this.stopFunctions.GetStopById(this.testStop.StopID);
        this.stopFunctions.DeleteStopById(stop.StopID);
        Assert.Throws<TadeoTDatabaseException>(() => this.stopFunctions.GetStopById(stop.StopID));
    }
}
