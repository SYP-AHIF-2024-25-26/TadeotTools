using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TadeoT.Database.Functions;
using TadeoT.Database;
using TadeoT.Database.Model;

namespace TadeoTUnitTests;

public class DivisionFunctionsUT {
    private readonly Division testDivision;

    [OneTimeSetUp]
    public void Setup() {
        DivisionFunctions.GetInstance().AddDivision(this.testDivision);
    }

    public DivisionFunctionsUT() {
        this.testDivision = new Division() {
            Name = "TestDivision",
            Color = "#FFFFFF"
        };
    }

    [Test, Order(1)]
    public void AddDivisionTest() {
        Division division = new() {
            Name = "TestDivision",
            Color = "#FFFFFF"
        };
        DivisionFunctions.GetInstance().AddDivision(division);
        Division result = DivisionFunctions.GetInstance().GetDivisionById(division.DivisionID);
        Assert.That(result, Is.Not.EqualTo(null));
    }

    [Test, Order(2)]
    public void GetDivisionByIdTest() {
        Division result = DivisionFunctions.GetInstance().GetDivisionById(testDivision.DivisionID);
        Assert.That(result.DivisionID, Is.EqualTo(testDivision.DivisionID));
    }

    [Test, Order(3)]
    public void UpdateDivisionTest() {
        Division division = DivisionFunctions.GetInstance().GetDivisionById(this.testDivision.DivisionID);
        division.Name = "Elektronik";
        DivisionFunctions.GetInstance().UpdateDivision(division);
        Division result = DivisionFunctions.GetInstance().GetDivisionById(this.testDivision.DivisionID);
        Assert.That(result.Name, Is.EqualTo("Elektronik"));
    }

    [Test, Order(4)]
    public void DeleteDivisionTest() {
        Division division = DivisionFunctions.GetInstance().GetDivisionById(this.testDivision.DivisionID);
        DivisionFunctions.GetInstance().DeleteDivisionById(division.DivisionID);
        Assert.Throws<TadeoTNotFoundException>(() => DivisionFunctions.GetInstance().GetDivisionById(this.testDivision.DivisionID));
    }
}

