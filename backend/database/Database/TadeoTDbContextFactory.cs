using Microsoft.EntityFrameworkCore;

namespace TadeoT.Database;

public class TadeoTDbContextFactory
{
    public TadeoTDbContext CreateDbContext(string[] args)
    {

        var optionsBuilder = new DbContextOptionsBuilder<TadeoTDbContext>();
        optionsBuilder.UseMySql(GetConnectionString(), ServerVersion.AutoDetect(GetConnectionString()));

        return new TadeoTDbContext(optionsBuilder.Options);
    }
    public static String GetConnectionString()
    {
        var config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();

        var serverName = config["Database:ServerName"];
        var serverPort = config["Database:Port"];
        var databaseName = config["Database:Name"];
        var username = config["Database:User"];
        var password = config["Database:Password"];

        return $"Server={serverName};Port={serverPort};Database={databaseName};User={username};Password={password};";
    }
}
