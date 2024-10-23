using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TadeoT.Database.Functions;
using TadeoT.Database;

namespace TadeoTUnitTests;

public class StopGroupFunctionsTests {
    private readonly TadeoTDbContext context = new();
    private readonly StopGroupFunctions stopGroupFunctions = new();
    private readonly StopGroup testGroup;

    [OneTimeSetUp]
    public void Setup() {
        this.stopGroupFunctions.AddStopGroup(this.testGroup);
    }

    public StopGroupFunctionsTests() {
        this.testGroup = new StopGroup() {
            StopGroupID = this.stopGroupFunctions.GetMaxId() + 1,
            Name = "TestGroup",
            Stops = [],
            Description = "TestDescription",
            Color = "#ffffff"
        };
    }
    
    [Test, Order(1)]
    public void AddStopGroupTest() {
        StopGroup group = new () {
            StopGroupID = this.stopGroupFunctions.GetMaxId() + 1,
            Name = "add group",
            Stops = [],
            Description = "TestDescription",
            Color = "#ffffff"
        };
        this.stopGroupFunctions.AddStopGroup(group);
        StopGroup result = this.stopGroupFunctions.GetStopGroupById(group.StopGroupID);
        Assert.That(result, Is.Not.EqualTo(null));
    }
    
    [Test, Order(2)]
    public void GetStopGroupByIdTest() {
        StopGroup result = this.stopGroupFunctions.GetStopGroupById(1);
        Assert.That(result.StopGroupID, Is.EqualTo(1));
    }

    [Test, Order(3)]
    public void UpdateStopGroupTest() {
        StopGroup group = this.stopGroupFunctions.GetStopGroupById(this.testGroup.StopGroupID);
        group.Name = "UpdatedName";
        this.stopGroupFunctions.UpdateStopGroup(group);
        StopGroup result = this.stopGroupFunctions.GetStopGroupById(this.testGroup.StopGroupID);
        Assert.That(result.Name, Is.EqualTo("UpdatedName"));
    }

    [Test, Order(4)]
    public void DeleteStopGroup() {
        StopGroup group = this.stopGroupFunctions.GetStopGroupById(this.testGroup.StopGroupID);
        this.stopGroupFunctions.DeleteStopGroupById(group.StopGroupID);
        Assert.Throws<TadeoTDatabaseException>(() => this.stopGroupFunctions.GetStopGroupById(this.testGroup.StopGroupID));
    }
}

