using TadeoT.Database;
using TadeoT.Database.Functions;


namespace TadeoTUnitTests;

public class StopFunctionsUT {
    private readonly TadeoTDbContext context = new();
    private readonly StopFunctions stopFunctions = new();
    private readonly StopGroupFunctions stopGroupFunctions = new();

    private readonly StopGroup testGroup;
    private readonly Stop testStop;

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

    [OneTimeSetUp]
    public void Setup() {
        this.stopFunctions.AddStop(this.testStop);
    }

    [Test, Order(1)]
    public void AddStopTest() {
        StopGroup group = new StopGroup() {
            StopGroupID = stopGroupFunctions.GetMaxId() + 1,
            Name = "add group",
            Stops = [],
            Description = "TestDescription",
            Color = "#ffffff"
        };
        this.stopGroupFunctions.AddStopGroup(group);
        StopGroup resultG = this.stopGroupFunctions.GetStopGroupById(group.StopGroupID);
        Assert.That(resultG, Is.Not.EqualTo(null));
    }

    [Test, Order(2)]
    public void GetStopByIdTest() {
        Stop result = this.stopFunctions.GetStopById(1);
        Assert.That(result.StopID, Is.EqualTo(1));
    }

    [Test, Order(3)]
    public void UpdateStopTest() {
        Stop stop = this.stopFunctions.GetStopById(this.testStop.StopID);
        stop.Name = "UpdatedName";
        this.stopFunctions.UpdateStop(stop);
        Stop result = this.stopFunctions.GetStopById(this.testStop.StopID);
        Assert.That(result.Name, Is.EqualTo("UpdatedName"));
    }

    [Test, Order(4)]
    public void DeleteStop() {
        Stop stop = this.stopFunctions.GetStopById(this.testStop.StopID);
        this.stopFunctions.DeleteStopById(stop.StopID);
        Assert.Throws<TadeoTDatabaseException>(() => this.stopFunctions.GetStopById(stop.StopID));
    }
}
