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
    private readonly Division testDivision;
    private readonly Stop testStop;

    public StopStatisticFunctionsTests() {
        testGroup = new StopGroup() {
            Name = "Informatik",
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
            StopGroup = testGroup,
            Division = testDivision
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
        StopGroupFunctions.GetInstance().AddStopGroup(this.testGroup);
        StopFunctions.GetInstance().AddStop(this.testStop);
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
        StopStatistic result = StopStatisticFunctions.GetInstance().GetStopStatisticById(testStatistic.StopStatisticID);
        Assert.That(result.StopStatisticID, Is.EqualTo(testStatistic.StopStatisticID));
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
        Assert.Throws<TadeoTNotFoundException>(() => StopStatisticFunctions.GetInstance().GetStopStatisticById(this.testStatistic.StopStatisticID));
    }
}

