using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TadeoT.Database.Functions;
using TadeoT.Database;
using TadeoT.Database.Model;

namespace TadeoTUnitTests;

public class DivisionFunctionsUT
{
    private readonly Division testDivision;
    private readonly DivisionFunctions divisionFunctions;


    [OneTimeSetUp]
    public void Setup()
    {
        divisionFunctions.AddDivision(this.testDivision).Wait();
    }

    public DivisionFunctionsUT(DivisionFunctions divisionFunctions)
    {
        this.divisionFunctions = divisionFunctions;

        this.testDivision = new Division()
        {
            Name = "TestDivision",
            Color = "#FFFFFF"
        };
    }

    [Test, Order(1)]
    public async Task AddDivisionTest()
    {
        Division division = new()
        {
            Name = "TestDivision",
            Color = "#FFFFFF"
        };
        await divisionFunctions.AddDivision(division);
        Division result = await divisionFunctions.GetDivisionById(division.DivisionID);
        Assert.That(result, Is.Not.EqualTo(null));
    }

    [Test, Order(2)]
    public async Task GetDivisionByIdTest()
    {
        Division result = await this.divisionFunctions.GetDivisionById(testDivision.DivisionID);
        Assert.That(result.DivisionID, Is.EqualTo(testDivision.DivisionID));
    }

    [Test, Order(3)]
    public async Task UpdateDivisionTest()
    {
        Division division = await this.divisionFunctions.GetDivisionById(this.testDivision.DivisionID);
        division.Name = "Elektronik";
        this.divisionFunctions.UpdateDivision(division);
        Division result = await this.divisionFunctions.GetDivisionById(this.testDivision.DivisionID);
        Assert.That(result.Name, Is.EqualTo("Elektronik"));
    }

    [Test, Order(4)]
    public async Task DeleteDivisionTest()
    {
        Division division = await this.divisionFunctions.GetDivisionById(this.testDivision.DivisionID);
        this.divisionFunctions.DeleteDivisionById(division.DivisionID);
        Assert.Throws<TadeoTNotFoundException>(async () => await this.divisionFunctions.GetDivisionById(this.testDivision.DivisionID));
    }
}

