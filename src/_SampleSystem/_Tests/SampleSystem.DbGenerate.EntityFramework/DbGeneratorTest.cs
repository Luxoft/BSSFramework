using Anch.Core;
using Anch.Testing.Xunit;

using Framework.Core;
using Framework.Database;
using Framework.Database.ConnectionStringSource;
using Framework.Database.Domain;

using Microsoft.Extensions.DependencyInjection;

using SampleSystem.DbGenerate.NHibernate;
using SampleSystem.ServiceEnvironment.DependencyInjection;

[assembly: AnchTestFramework]

namespace SampleSystem.DbGenerate.EntityFramework;

public class DbGeneratorTest
{
    [AnchFact]
    public Task GenerateLocal(CancellationToken ct) => this.GenerateAllDb(@".", nameof(SampleSystem), null, ct);

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

        await this.GenerateAllDb(options.Server, options.DataBase, null, ct);
    }

    public async Task GenerateAllDb(string serverName, string mainDatabaseName, DbUserCredential? credentials, CancellationToken ct)
    {
        var credStr = credentials == null ? "Integrated Security=True" : $"User ID={credentials.UserName};Password={credentials.Password}";

        var connectionString =
            $"Data Source={serverName};Initial Catalog={mainDatabaseName};{credStr};Application Name=SampleSystem;TrustServerCertificate=true";

        var rootServiceProvider = new ServiceCollection()
                                  .AddSingleton<IDefaultConnectionStringSource>(new ManualDefaultConnectionStringSource(connectionString))
                                  .AddSingleton(DBSessionSettings.Default)
                                  .Self(new SampleSystemEntityFrameworkExtension().AddServices)
                                  .BuildServiceProvider(new ServiceProviderOptions { ValidateScopes = true, ValidateOnBuild = true });

        await using var scope = rootServiceProvider.CreateAsyncScope();

        await using var dbContext = scope.ServiceProvider.GetRequiredService<SampleSystemDbContext>();
        await dbContext.Database.EnsureCreatedAsync(ct);

        new BssFluentMigrator(connectionString).Migrate();
    }
}
