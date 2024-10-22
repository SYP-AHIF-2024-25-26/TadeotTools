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

    public StopGroupFunctionsTests() {
        testGroup = new StopGroup() {
            StopGroupID = this.stopGroupFunctions.GetMaxId() + 1,
            Name = "TestGroup",
            Stops = [],
            Description = "TestDescription",
            Color = "#ffffff"
        };
    }
    [Test, Order(1)]
    public void AddStopGroupTest() {
        this.stopGroupFunctions.AddStopGroup(this.testGroup);
        StopGroup result = this.stopGroupFunctions.GetStopGroupById(this.testGroup.StopGroupID);
        Assert.That(result != null, Is.True);
    }

    [Test]
    public void GetStopGroupByIdTest() {
        StopGroup result = this.stopGroupFunctions.GetStopGroupById(1);
        Assert.That(result.StopGroupID, Is.EqualTo(1));
    }

    [Test]
    public void UpdateStopGroupTest() {
        StopGroup group = this.stopGroupFunctions.GetStopGroupById(this.testGroup.StopGroupID);
        group.Name = "UpdatedName";
        this.stopGroupFunctions.UpdateStopGroup(group);
        StopGroup result = this.stopGroupFunctions.GetStopGroupById(this.testGroup.StopGroupID);
        Assert.That(result.Name, Is.EqualTo("UpdatedName"));
    }

    [Test]
    public void DeleteStopGroup() {
        StopGroup group = this.stopGroupFunctions.GetStopGroupById(this.testGroup.StopGroupID);
        this.stopGroupFunctions.DeleteStopGroupById(group.StopGroupID);
        Assert.Throws<Exception>(() => this.stopGroupFunctions.GetStopGroupById(this.testGroup.StopGroupID));
    }
}

