﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TadeoT.Database.Functions;
using TadeoT.Database;

namespace TadeoTUnitTests;

public class StopGroupFunctionsTests {
    private readonly StopGroup testGroup;

    [OneTimeSetUp]
    public void Setup() {
        StopGroupFunctions.GetInstance().AddStopGroup(this.testGroup);
    }

    public StopGroupFunctionsTests() {
        this.testGroup = new StopGroup() {
            StopGroupID = StopGroupFunctions.GetInstance().GetMaxId() + 1,
            Name = "TestGroup",
            Stops = [],
            Description = "TestDescription",
            Color = "#ffffff"
        };
    }
    
    [Test, Order(1)]
    public void AddStopGroupTest() {
        StopGroup group = new () {
            StopGroupID = StopGroupFunctions.GetInstance().GetMaxId() + 1,
            Name = "add group",
            Stops = [],
            Description = "TestDescription",
            Color = "#ffffff"
        };
        StopGroupFunctions.GetInstance().AddStopGroup(group);
        StopGroup result = StopGroupFunctions.GetInstance().GetStopGroupById(group.StopGroupID);
        Assert.That(result, Is.Not.EqualTo(null));
    }
    
    [Test, Order(2)]
    public void GetStopGroupByIdTest() {
        StopGroup result = StopGroupFunctions.GetInstance().GetStopGroupById(1);
        Assert.That(result.StopGroupID, Is.EqualTo(1));
    }

    [Test, Order(3)]
    public void UpdateStopGroupTest() {
        StopGroup group = StopGroupFunctions.GetInstance().GetStopGroupById(this.testGroup.StopGroupID);
        group.Name = "UpdatedName";
        StopGroupFunctions.GetInstance().UpdateStopGroup(group);
        StopGroup result = StopGroupFunctions.GetInstance().GetStopGroupById(this.testGroup.StopGroupID);
        Assert.That(result.Name, Is.EqualTo("UpdatedName"));
    }

    [Test, Order(4)]
    public void DeleteStopGroup() {
        StopGroup group = StopGroupFunctions.GetInstance().GetStopGroupById(this.testGroup.StopGroupID);
        StopGroupFunctions.GetInstance().DeleteStopGroupById(group.StopGroupID);
        Assert.Throws<TadeoTDatabaseException>(() => StopGroupFunctions.GetInstance().GetStopGroupById(this.testGroup.StopGroupID));
    }
}

