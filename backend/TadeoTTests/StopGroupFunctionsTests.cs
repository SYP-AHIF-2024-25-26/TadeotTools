using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TadeoT.Database.Functions;
using TadeoT.Database;
using Xunit.Priority;

namespace TadeoTTests;
public class StopGroupFunctionsTests {
    private readonly TadeoTDbContext context = new();
    private readonly StopGroupFunctions stopGroupFunctions = new();
    private StopGroup testGroup;

    public StopGroupFunctionsTests() {
        testGroup = new StopGroup() {
            StopGroupID = stopGroupFunctions.GetMaxId() + 1,
            Name = "TestGroup",
            Stops = [],
            Description = "TestDescription",
            Color = "#ffffff"
        };
    }

    [Fact, Priority(1)]
    public void GetStopGroupByIdTest() {
        StopGroup result = this.stopGroupFunctions.GetStopGroupById(1);
        Assert.Equal(1, result.StopGroupID);
    }

    [Fact, Priority(2)]
    public void AddStopGroupTest() {
        this.stopGroupFunctions.AddStopGroup(this.testGroup);
        StopGroup result = this.stopGroupFunctions.GetStopGroupById(this.testGroup.StopGroupID);
        Assert.True(result != null);
    }

    [Fact]
    public void UpdateStopGroupTest() {
        try {
            StopGroup group = this.stopGroupFunctions.GetStopGroupById(this.testGroup.StopGroupID);
            group.Name = "UpdatedName";
            this.stopGroupFunctions.UpdateStopGroup(group);
            StopGroup result = this.stopGroupFunctions.GetStopGroupById(this.testGroup.StopGroupID);
            Assert.Equal("UpdatedName", result.Name);
        } catch (Exception e) {
            Console.WriteLine("Could not update Stop: " + e.Message);
        }

    }

    [Fact]
    public void DeleteStopGroup() {
        try {
            StopGroup group = this.stopGroupFunctions.GetStopGroupById(this.testGroup.StopGroupID);
            this.stopGroupFunctions.DeleteStopGroupById(group.StopGroupID);
            Assert.Throws<Exception>(() => this.stopGroupFunctions.GetStopGroupById(testGroup.StopGroupID));
        } catch (Exception e) {
            Console.WriteLine("Could not delete StopGroup: " + e.Message);
        }
    }
}

