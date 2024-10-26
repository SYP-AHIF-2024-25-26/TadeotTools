using TadeoT.Database;
using TadeoT.Database.Functions;


namespace TadeoTUnitTests;

public class StopFunctionsUT {
    private readonly TadeoTDbContext context = new();

    private readonly StopGroup testGroup;
    private readonly Stop testStop;

    public StopFunctionsUT() {
        testGroup = new StopGroup() {
            StopGroupID = StopGroupFunctions.GetInstance().GetMaxId() + 1,
            Name = "TestGroup",
            Description = "TestDescription",
            Color = "#ffffff"
        };
        testStop = new Stop() {
            Name = "TestStop",
            Description = "TestDescription",
            RoomNr = "E72",
            StopID = StopFunctions.GetInstance().GetMaxId() + 1,
            StopGroup = testGroup,
            StopStatistics = []
        };
    }

    [OneTimeSetUp]
    public void Setup() {
        StopFunctions.GetInstance().AddStop(this.testStop);
    }

    [Test, Order(1)]
    public void AddStopTest() {
        StopGroup group = new StopGroup() {
            StopGroupID = StopGroupFunctions.GetInstance().GetMaxId() + 1,
            Name = "add group",
            Description = "TestDescription",
            Color = "#ffffff"
        };
        StopGroupFunctions.GetInstance().AddStopGroup(group);
        StopGroup resultG = StopGroupFunctions.GetInstance().GetStopGroupById(group.StopGroupID);
        Assert.That(resultG, Is.Not.EqualTo(null));
    }

    [Test, Order(2)]
    public void GetStopByIdTest() {
        Stop result = StopFunctions.GetInstance().GetStopById(1);
        Assert.That(result.StopID, Is.EqualTo(1));
    }

    [Test, Order(3)]
    public void UpdateStopTest() {
        Stop stop = StopFunctions.GetInstance().GetStopById(this.testStop.StopID);
        stop.Name = "UpdatedName";
        StopFunctions.GetInstance().UpdateStop(stop);
        Stop result = StopFunctions.GetInstance().GetStopById(this.testStop.StopID);
        Assert.That(result.Name, Is.EqualTo("UpdatedName"));
    }

    [Test, Order(4)]
    public void DeleteStop() {
        Stop stop = StopFunctions.GetInstance().GetStopById(this.testStop.StopID);
        StopFunctions.GetInstance().DeleteStopById(stop.StopID);
        Assert.Throws<TadeoTDatabaseException>(() => StopFunctions.GetInstance().GetStopById(stop.StopID));
    }
}
