using Anch.Core;

using Framework.Core;
using Framework.Database;
using Framework.Database.ConnectionStringSource;
using Framework.Database.EntityFramework.Audit.DependencyInjection;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

using SampleSystem.DbGenerate.NHibernate;
using SampleSystem.ServiceEnvironment.DependencyInjection;

namespace SampleSystem.DbGenerate.EntityFramework;

public class DbGeneratorTest
{
    [Fact]
    public Task GenerateLocal() => this.GenerateAllDb(@".", nameof(SampleSystem), TestContext.Current.CancellationToken);

    public async Task GenerateDatabase(DbGenerationOptions options)
    {
        var ct = TestContext.Current.CancellationToken;

        if (string.IsNullOrWhiteSpace(options.Server))
        {
            throw new ArgumentException("Server name is empty");
        }

        if (string.IsNullOrWhiteSpace(options.DataBase))
        {
            throw new ArgumentException("DataBase name is empty");
        }

        Console.WriteLine($"Generate database:'{options.DataBase}' on {options.Server}");

        await this.GenerateAllDb(options.Server, options.DataBase, ct);
    }

    public async Task GenerateAllDb(string serverName, string mainDatabaseName, CancellationToken ct)
    {
        var connectionString =
            $"Data Source={serverName};Initial Catalog={mainDatabaseName};Integrated Security=True;Application Name=SampleSystem;TrustServerCertificate=true";

        var rootServiceProvider = new ServiceCollection()
                                  .AddSingleton<IDefaultConnectionStringSource>(new ManualDefaultConnectionStringSource(connectionString))
                                  .Self(new SampleSystemEntityFrameworkExtension().AddServices)
                                  .BuildServiceProvider(new ServiceProviderOptions { ValidateScopes = true, ValidateOnBuild = true });

        await using var scope = rootServiceProvider.CreateAsyncScope();

        await using var dbContext = scope.ServiceProvider.GetRequiredService<SampleSystemDbContext>();
        await dbContext.Database.EnsureCreatedAsync(ct);

        new BssFluentMigrator(connectionString).Migrate();
    }
}
