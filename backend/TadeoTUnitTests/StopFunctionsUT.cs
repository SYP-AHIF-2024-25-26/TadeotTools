using TadeoT.Database;
using TadeoT.Database.Functions;


namespace TadeoTUnitTests;

public class StopFunctionsUT {
    private readonly TadeoTDbContext context = new();

    private readonly StopGroup testGroup;
    private readonly Stop testStop;
    private readonly Division testDivision;

    public StopFunctionsUT() {
        testGroup = new StopGroup() {
            Name = "TestGroup",
            Description = "TestDescription",
        };

        testDivision = new Division() {
            Name = "TestDivision",
            Color = "#FFFFFF"
        };

        testStop = new Stop() {
            Name = "TestStop",
            Description = "TestDescription",
            RoomNr = "E72",
            Division = testDivision,
            StopGroup = testGroup
        };
    }

    [OneTimeSetUp]
    public void Setup() {
        StopGroupFunctions.GetInstance().AddStopGroup(this.testGroup);
        DivisionFunctions.GetInstance().AddDivision(this.testDivision);
        StopFunctions.GetInstance().AddStop(this.testStop);
    }

    [Test, Order(1)]
    public void AddStopTest() {
        Stop stop = new () {
            Name = "add stop",
            Description = "TestDescription",
            RoomNr = "E72",
            StopGroup = this.testGroup,
            Division = this.testDivision
        };
        StopFunctions.GetInstance().AddStop(stop);
        Stop result = StopFunctions.GetInstance().GetStopById(stop.StopID);
        Assert.That(result, Is.Not.EqualTo(null));
    }

    [Test, Order(2)]
    public void GetStopByIdTest() {
        Stop result = StopFunctions.GetInstance().GetStopById(testStop.StopID);
        Assert.That(result.StopID, Is.EqualTo(testStop.StopID));
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
        Assert.Throws<TadeoTNotFoundException>(() => StopFunctions.GetInstance().GetStopById(stop.StopID));
    }
}