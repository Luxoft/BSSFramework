using Framework.Database.EntityFramework.Audit.DependencyInjection;

using Microsoft.EntityFrameworkCore;

using SampleSystem.DbGenerate.NHibernate;
using SampleSystem.ServiceEnvironment.DependencyInjection;

namespace SampleSystem.DbGenerate.EntityFramework;

public class DbGeneratorTest
{
    [Fact]
    public void GenerateLocal() => this.GenerateAllDB(@".");

    public void GenerateDatabase(DbGenerationOptions options)
    {
        if (string.IsNullOrWhiteSpace(options.Server))
        {
            throw new ArgumentException("Server name is empty");
        }

        if (string.IsNullOrWhiteSpace(options.DataBase))
        {
            throw new ArgumentException("DataBase name is empty");
        }

        Console.WriteLine($"Generate database:'{options.DataBase}' on {options.Server}");

        this.GenerateAllDB(options.Server, options.DataBase);
    }

    public void GenerateAllDB(string serverName, string mainDatabaseName = nameof(SampleSystem))
    {
        var connectionString = $"Data Source={serverName};Initial Catalog={mainDatabaseName};Application Name=SampleSystem";
        var optionsBuilder = new DbContextOptionsBuilder<SampleSystemDbContext>();
        optionsBuilder.UseSqlServer(connectionString);
        optionsBuilder.AddAudit();

        using var context = new SampleSystemDbContext(optionsBuilder.Options);
        context.Database.EnsureCreated();
    }
}
