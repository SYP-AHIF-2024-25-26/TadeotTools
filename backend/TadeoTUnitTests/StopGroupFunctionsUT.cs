using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TadeoT.Database.Functions;
using TadeoT.Database;
using TadeoT.Database.Model;

namespace TadeoTUnitTests;

public class StopGroupFunctionsTests {
    private readonly StopGroup testGroup;

    [OneTimeSetUp]
    public void Setup() {
        StopGroupFunctions.GetInstance().AddStopGroup(this.testGroup);
    }

    public StopGroupFunctionsTests() {
        this.testGroup = new StopGroup() {
            Name = "TestName",
            Description = "TestDescription",
        };
    }
    
    [Test, Order(1)]
    public void AddStopGroupTest() {
        StopGroup group = new () {
            Name = "TestName",
            Description = "TestDescription",
        };
        StopGroupFunctions.GetInstance().AddStopGroup(group);
        StopGroup result = StopGroupFunctions.GetInstance().GetStopGroupById(group.StopGroupID);
        Assert.That(result, Is.Not.EqualTo(null));
    }
    
    [Test, Order(2)]
    public void GetStopGroupByIdTest() {
        StopGroup result = StopGroupFunctions.GetInstance().GetStopGroupById(testGroup.StopGroupID);
        Assert.That(result.StopGroupID, Is.EqualTo(testGroup.StopGroupID));
    }

    [Test, Order(3)]
    public void UpdateStopGroupTest() {
        StopGroup group = StopGroupFunctions.GetInstance().GetStopGroupById(this.testGroup.StopGroupID);
        group.Name = "Elektronik";
        StopGroupFunctions.GetInstance().UpdateStopGroup(group);
        StopGroup result = StopGroupFunctions.GetInstance().GetStopGroupById(this.testGroup.StopGroupID);
        Assert.That(result.Name, Is.EqualTo("Elektronik"));
    }

    [Test, Order(4)]
    public void DeleteStopGroup() {
        StopGroup group = StopGroupFunctions.GetInstance().GetStopGroupById(this.testGroup.StopGroupID);
        StopGroupFunctions.GetInstance().DeleteStopGroupById(group.StopGroupID);
        Assert.Throws<TadeoTNotFoundException>(() => StopGroupFunctions.GetInstance().GetStopGroupById(this.testGroup.StopGroupID));
    }
}
