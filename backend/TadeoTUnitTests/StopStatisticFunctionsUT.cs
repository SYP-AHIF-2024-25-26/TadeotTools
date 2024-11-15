using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TadeoT.Database.Functions;
using TadeoT.Database;
using TadeoT.Database.Model;

namespace TadeoTUnitTests;

public class StopStatisticFunctionsTests
{
    private readonly StopStatistic testStatistic;
    private readonly StopGroup testGroup;
    private readonly Division testDivision;
    private readonly Stop testStop;

    private readonly StopStatisticFunctions stopStatisticFunctions;
    private readonly StopGroupFunctions stopGroupFunctions;
    private readonly DivisionFunctions divisionFunctions;
    private readonly StopFunctions stopFunctions;

    public StopStatisticFunctionsTests(StopStatisticFunctions stopStatisticFunctions, StopGroupFunctions stopGroupFunctions, DivisionFunctions divisionFunctions, StopFunctions stopFunctions)
    {
        testGroup = new StopGroup()
        {
            Name = "Informatik",
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
            StopGroup = testGroup,
            Division = testDivision
        };
        testStatistic = new StopStatistic()
        {
            StopID = this.testStop.StopID,
            Stop = this.testStop,
            Time = DateTime.Now,
            IsDone = false
        };
        this.stopStatisticFunctions = stopStatisticFunctions;
        this.stopGroupFunctions = stopGroupFunctions;
        this.divisionFunctions = divisionFunctions;
        this.stopFunctions = stopFunctions;
    }

    [OneTimeSetUp]
    public void Setup()
    {
        Task.Run(() => this.divisionFunctions.AddDivision(this.testDivision));
        Task.Run(() => this.stopGroupFunctions.AddStopGroup(this.testGroup));
        Task.Run(() => this.stopFunctions.AddStop(this.testStop));
        Task.Run(() => this.stopStatisticFunctions.AddStopStatistic(this.testStatistic));
    }

    [Test, Order(1)]
    public async Task AddStopStatisticTest()
    {
        StopStatistic stopStatistic = new()
        {
            StopID = this.testStop.StopID,
            Stop = this.testStop,
            Time = DateTime.Now,
            IsDone = false
        };
        await this.stopStatisticFunctions.AddStopStatistic(stopStatistic);
        StopStatistic result = await this.stopStatisticFunctions.GetStopStatisticById(stopStatistic.StopStatisticID);
        Assert.That(result, Is.Not.EqualTo(null));
    }

    [Test, Order(2)]
    public async Task GetStopStatisticByIdTest()
    {
        StopStatistic result = await this.stopStatisticFunctions.GetStopStatisticById(testStatistic.StopStatisticID);
        Assert.That(result.StopStatisticID, Is.EqualTo(testStatistic.StopStatisticID));
    }

    [Test, Order(3)]
    public async Task UpdateStopStatisticTest()
    {
        StopStatistic stat = await this.stopStatisticFunctions.GetStopStatisticById(this.testStatistic.StopStatisticID);
        stat.IsDone = true;
        this.stopStatisticFunctions.UpdateStopStatistic(stat);
        StopStatistic result = await this.stopStatisticFunctions.GetStopStatisticById(this.testStatistic.StopStatisticID);
        Assert.That(result.IsDone, Is.EqualTo(true));
    }

    [Test, Order(4)]
    public async Task DeleteStopStatistic()
    {
        StopStatistic stat = await this.stopStatisticFunctions.GetStopStatisticById(this.testStatistic.StopStatisticID);
        this.stopStatisticFunctions.DeleteStopStopStatisticById(stat.StopStatisticID);
        Assert.Throws<TadeoTNotFoundException>(async () => await this.stopStatisticFunctions.GetStopStatisticById(this.testStatistic.StopStatisticID));
    }
}

