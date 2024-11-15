using TadeoT.Database;
using TadeoT.Database.Functions;
using TadeoT.Database.Model;


namespace TadeoTUnitTests;

public class StopFunctionsUT
{
    private readonly StopGroup testGroup;
    private readonly Stop testStop;
    private readonly Division testDivision;

    private readonly DivisionFunctions divisionFunctions;
    private readonly StopFunctions stopFunctions;
    private readonly StopGroupFunctions stopGroupFunctions;

    public StopFunctionsUT(DivisionFunctions divisionFunctions, StopFunctions stopFunctions, StopGroupFunctions stopGroupFunctions)
    {
        this.stopFunctions = stopFunctions;
        this.divisionFunctions = divisionFunctions;
        this.stopGroupFunctions = stopGroupFunctions;

        testGroup = new StopGroup()
        {
            Name = "TestGroup",
            Description = "TestDescription",
            IsPublic = true
        };

        testDivision = new Division()
        {
            Name = "TestDivision",
            Color = "#FFFFFF"
        };

        testStop = new Stop()
        {
            Name = "TestStop",
            Description = "TestDescription",
            RoomNr = "E72",
            Division = testDivision,
            StopGroup = testGroup
        };
    }

    [OneTimeSetUp]
    public void Setup()
    {
        this.stopGroupFunctions.AddStopGroup(this.testGroup).Wait();
        this.divisionFunctions.AddDivision(this.testDivision).Wait();
        this.stopFunctions.AddStop(this.testStop).Wait();
    }

    [Test, Order(1)]
    public async Task AddStopTest()
    {
        Stop stop = new()
        {
            Name = "add stop",
            Description = "TestDescription",
            RoomNr = "E72",
            StopGroup = this.testGroup,
            Division = this.testDivision
        };
        await this.stopFunctions.AddStop(stop);
        Stop result = await this.stopFunctions.GetStopById(stop.StopID);
        Assert.That(result, Is.Not.EqualTo(null));
    }

    [Test, Order(2)]
    public async Task GetStopByIdTest()
    {
        Stop result = await this.stopFunctions.GetStopById(testStop.StopID);
        Assert.That(result.StopID, Is.EqualTo(testStop.StopID));
    }

    [Test, Order(3)]
    public async Task UpdateStopTest()
    {
        Stop stop = await this.stopFunctions.GetStopById(this.testStop.StopID);
        stop.Name = "UpdatedName";
        this.stopFunctions.UpdateStop(stop);
        Stop result = await this.stopFunctions.GetStopById(this.testStop.StopID);
        Assert.That(result.Name, Is.EqualTo("UpdatedName"));
    }

    [Test, Order(4)]
    public async Task DeleteStop()
    {
        Stop stop = await this.stopFunctions.GetStopById(this.testStop.StopID);
        this.stopFunctions.DeleteStopById(stop.StopID);
        Assert.Throws<TadeoTNotFoundException>(async () => await this.stopFunctions.GetStopById(stop.StopID));
    }
}