using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TadeoT.Database.Functions;
using TadeoT.Database;
using TadeoT.Database.Model;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

[assembly: NonParallelizable]

namespace TadeoTUnitTests;

[TestFixture]
public class DivisionFunctionsUT
{
    private readonly Division testDivision;

    private ServiceProvider? ServiceProvider = null;

    private static ServiceProvider BuildServiceProvider()
    {
        var services = new ServiceCollection();
        services.AddDbContext<TadeoTDbContext>(options =>
            options.UseMySql(TadeoTDbContextFactory.GetConnectionString(), ServerVersion.AutoDetect(TadeoTDbContextFactory.GetConnectionString())));
        services.AddScoped<DivisionFunctions>();

        return services.BuildServiceProvider();
    }

    [OneTimeSetUp]
    public void Setup()
    {
        using var scope = ServiceProvider!.CreateScope();
        var divisionFunctions = scope.ServiceProvider.GetRequiredService<DivisionFunctions>();

        ServiceProvider = BuildServiceProvider();

        var context = scope.ServiceProvider.GetRequiredService<TadeoTDbContext>();
        context.Database.EnsureCreatedAsync().Wait();
        context.Divisions.ExecuteDeleteAsync().Wait();

        divisionFunctions.AddDivision(this.testDivision).Wait();
    }

    [OneTimeTearDown]
    public void TearDown()
    {
        ServiceProvider?.Dispose();
    }

    public DivisionFunctionsUT()
    {
        this.testDivision = new Division()
        {
            Name = "TestDivision",
            Color = "#FFFFFF"
        };
    }

    [Test, Order(1)]
    public async Task AddDivisionTest()
    {
        using var scope = ServiceProvider!.CreateScope();
        var divisionFunctions = scope.ServiceProvider.GetRequiredService<DivisionFunctions>();
        
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
        using var scope = ServiceProvider!.CreateScope();
        var divisionFunctions = scope.ServiceProvider.GetRequiredService<DivisionFunctions>();
        Division result = await divisionFunctions.GetDivisionById(testDivision.DivisionID);
        Assert.That(result.DivisionID, Is.EqualTo(testDivision.DivisionID));
    }

    [Test, Order(3)]
    public async Task UpdateDivisionTest()
    {
        using var scope = ServiceProvider!.CreateScope();
        var divisionFunctions = scope.ServiceProvider.GetRequiredService<DivisionFunctions>();
        Division division = await divisionFunctions.GetDivisionById(this.testDivision.DivisionID);
        division.Name = "Elektronik";
        divisionFunctions.UpdateDivision(division);
        Division result = await divisionFunctions.GetDivisionById(this.testDivision.DivisionID);
        Assert.That(result.Name, Is.EqualTo("Elektronik"));
    }

    [Test, Order(4)]
    public async Task DeleteDivisionTest()
    {
        using var scope = ServiceProvider!.CreateScope();
        var divisionFunctions = scope.ServiceProvider.GetRequiredService<DivisionFunctions>();
        Division division = await divisionFunctions.GetDivisionById(this.testDivision.DivisionID);
        divisionFunctions.DeleteDivisionById(division.DivisionID);
        Assert.Throws<TadeoTNotFoundException>(async () => await divisionFunctions.GetDivisionById(this.testDivision.DivisionID));
    }
}

