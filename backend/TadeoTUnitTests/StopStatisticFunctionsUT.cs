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

    private readonly StopStatistic testStatistic;
    private readonly StopGroup testGroup;
    private readonly Stop testStop;

    public StopStatisticFunctionsTests() {
        testGroup = new StopGroup() {
            Name = StopGroupName.Informatik,
            Description = "TestDescription",
            Color = "#ffffff"
        };
        testStop = new Stop() {
            Name = "TestStop",
            Description = "TestDescription",
            RoomNr = "E72",
            StopGroup = testGroup,
        };
        testStatistic = new StopStatistic() {
            StopID = this.testStop.StopID,
            Stop = this.testStop,
            Time = DateTime.Now,
            IsDone = false
        };
    }

    [OneTimeSetUp]
    public void Setup() {
        StopStatisticFunctions.GetInstance().AddStopStatistic(this.testStatistic);
    }

    [Test, Order(1)]
    public void AddStopStatisticTest() {
        StopStatistic stopStatistic = new() {
            StopID = this.testStop.StopID,
            Stop = this.testStop,
            Time = DateTime.Now,
            IsDone = false
        };
        StopStatisticFunctions.GetInstance().AddStopStatistic(stopStatistic);
        StopStatistic result = StopStatisticFunctions.GetInstance().GetStopStatisticById(stopStatistic.StopStatisticID);
        Assert.That(result, Is.Not.EqualTo(null));
    }

    [Test, Order(2)]
    public void GetStopStatisticByIdTest() {
        StopStatistic result = StopStatisticFunctions.GetInstance().GetStopStatisticById(1);
        Assert.That(result.StopStatisticID, Is.EqualTo(1));
    }

    [Test, Order(3)]
    public void UpdateStopStatisticTest() {
        StopStatistic stat = StopStatisticFunctions.GetInstance().GetStopStatisticById(this.testStatistic.StopStatisticID);
        stat.IsDone = true;
        StopStatisticFunctions.GetInstance().UpdateStopStatistic(stat);
        StopStatistic result = StopStatisticFunctions.GetInstance().GetStopStatisticById(this.testStatistic.StopStatisticID);
        Assert.That(result.IsDone, Is.EqualTo(true));
    }

    [Test, Order(4)]
    public void DeleteStopStatistic() {
        StopStatistic stat = StopStatisticFunctions.GetInstance().GetStopStatisticById(this.testStatistic.StopStatisticID);
        StopStatisticFunctions.GetInstance().DeleteStopStopStatisticById(stat.StopStatisticID);
        Assert.Throws<TadeoTDatabaseException>(() => StopStatisticFunctions.GetInstance().GetStopStatisticById(this.testStatistic.StopStatisticID));
    }
}

