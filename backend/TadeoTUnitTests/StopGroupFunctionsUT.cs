using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TadeoT.Database.Functions;
using TadeoT.Database;
using TadeoT.Database.Model;

namespace TadeoTUnitTests;

public class StopGroupFunctionsTests
{
    private readonly StopGroup testGroup;
    private readonly StopGroupFunctions stopGroupFunctions;

    [OneTimeSetUp]
    public void Setup()
    {
        Task.Run(() => this.stopGroupFunctions.AddStopGroup(this.testGroup));
    }

    public StopGroupFunctionsTests(StopGroupFunctions stopGroupFunctions)
    {
        this.stopGroupFunctions = stopGroupFunctions;
        this.testGroup = new StopGroup()
        {
            Name = "TestName",
            Description = "TestDescription",
            IsPublic = true
        };
    }

    [Test, Order(1)]
    public async Task AddStopGroupTest()
    {
        StopGroup group = new()
        {
            Name = "TestName",
            Description = "TestDescription",
            IsPublic = true
        };
        await this.stopGroupFunctions.AddStopGroup(group);
        StopGroup result = await this.stopGroupFunctions.GetStopGroupById(group.StopGroupID);
        Assert.That(result, Is.Not.EqualTo(null));
    }

    [Test, Order(2)]
    public async Task GetStopGroupByIdTest()
    {
        StopGroup result = await this.stopGroupFunctions.GetStopGroupById(testGroup.StopGroupID);
        Assert.That(result.StopGroupID, Is.EqualTo(testGroup.StopGroupID));
    }

    [Test, Order(3)]
    public async Task UpdateStopGroupTest()
    {
        StopGroup group = await this.stopGroupFunctions.GetStopGroupById(this.testGroup.StopGroupID);
        group.Name = "Elektronik";
        this.stopGroupFunctions.UpdateStopGroup(group);
        StopGroup result = await this.stopGroupFunctions.GetStopGroupById(this.testGroup.StopGroupID);
        Assert.That(result.Name, Is.EqualTo("Elektronik"));
    }

    [Test, Order(4)]
    public async Task DeleteStopGroup()
    {
        StopGroup group = await this.stopGroupFunctions.GetStopGroupById(this.testGroup.StopGroupID);
        this.stopGroupFunctions.DeleteStopGroupById(group.StopGroupID);
        Assert.Throws<TadeoTNotFoundException>(async () => await this.stopGroupFunctions.GetStopGroupById(this.testGroup.StopGroupID));
    }
}
