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
    [Test, Order(1)]
    public void AddStopStatisticTest() {
        this.stopGroupFunctions.AddStopGroup(this.testGroup);
        this.stopFunctions.AddStop(this.testStop);
        this.stopStatisticFunctions.AddStopStatistic(this.testStatistic);
        StopStatistic result = this.stopStatisticFunctions.GetStopStatisticById(this.testStatistic.StopStatisticID);
        Assert.That(result != null, Is.True);
    }

    [Test]
    public void GetStopStatisticByIdTest() {
        StopStatistic result = this.stopStatisticFunctions.GetStopStatisticById(1);
        Assert.That(result.StopStatisticID, Is.EqualTo(1));
    }

    [Test]
    public void UpdateStopStatisticTest() {
        StopStatistic stat = this.stopStatisticFunctions.GetStopStatisticById(this.testGroup.StopGroupID);
        stat.IsDone = true;
        this.stopStatisticFunctions.UpdateStopStatistic(stat);
        StopGroup result = this.stopGroupFunctions.GetStopGroupById(this.testGroup.StopGroupID);
        Assert.That(result.Name, Is.EqualTo("UpdatedName"));
    }

    [Test]
    public void DeleteStopStatistic() {
        StopStatistic stat = this.stopStatisticFunctions.GetStopStatisticById(this.testGroup.StopGroupID);
        this.stopGroupFunctions.DeleteStopGroupById(stat.StopStatisticID);
        Assert.Throws<TadeoTDatabaseException>(() => this.stopStatisticFunctions.GetStopStatisticById(this.testGroup.StopGroupID));
    }
}

