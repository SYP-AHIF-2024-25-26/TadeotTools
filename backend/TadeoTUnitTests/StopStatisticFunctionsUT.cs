using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TadeoT.Database.Functions;
using TadeoT.Database;

namespace TadeoTUnitTests;

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
            StopID = this.testStop.StopID,
            Stop = this.testStop,
            Time = DateTime.Now,
            IsDone = false
        };
    }

    [OneTimeSetUp]
    public void Setup() {
        // this.stopStatisticFunctions.AddStopStatistic(this.testStatistic);
    }

    [Test, Order(1)]
    public void AddStopStatisticTest() {
        StopStatistic stopStatistic = new StopStatistic() {
            StopStatisticID = this.stopStatisticFunctions.GetMaxId() + 1,
            StopID = this.testStop.StopID,
            Stop = this.testStop,
            Time = DateTime.Now,
            IsDone = false
        };
        this.stopStatisticFunctions.AddStopStatistic(stopStatistic);
        StopStatistic result = this.stopStatisticFunctions.GetStopStatisticById(stopStatistic.StopStatisticID);
        Assert.That(result, Is.Not.EqualTo(null));
    }

    [Test, Order(2)]
    public void GetStopStatisticByIdTest() {
        StopStatistic result = this.stopStatisticFunctions.GetStopStatisticById(1);
        Assert.That(result.StopStatisticID, Is.EqualTo(1));
    }

    [Test, Order(3)]
    public void UpdateStopStatisticTest() {
        StopStatistic stat = this.stopStatisticFunctions.GetStopStatisticById(this.testStatistic.StopStatisticID);
        stat.IsDone = true;
        this.stopStatisticFunctions.UpdateStopStatistic(stat);
        StopStatistic result = this.stopStatisticFunctions.GetStopStatisticById(this.testStatistic.StopStatisticID);
        Assert.That(result.IsDone, Is.EqualTo(true));
    }

    [Test, Order(4)]
    public void DeleteStopStatistic() {
        StopStatistic stat = this.stopStatisticFunctions.GetStopStatisticById(this.testStatistic.StopStatisticID);
        this.stopStatisticFunctions.DeleteStopStopStatisticById(stat.StopStatisticID);
        Assert.Throws<TadeoTDatabaseException>(() => this.stopStatisticFunctions.GetStopStatisticById(this.testStatistic.StopStatisticID));
    }
}

