using Anch.Core;
using Anch.Testing.Database.ConnectionStringManagement;

using Framework.AutomationCore.Extensions;

using SampleSystem.DbGenerate.NHibernate;
using SampleSystem.IntegrationTests._Environment.FluentMigration;

namespace SampleSystem.IntegrationTests._Environment;

public class EmptySchemaInitializer(IActualTestConnectionStringSource actualTestConnectionStringSource) : IInitializer
{
    public async Task Initialize(CancellationToken ct)
    {
        var generator = new DbGeneratorTest();

        generator.GenerateAllDB(
            actualTestConnectionStringSource.ActualConnectionString.DataSource,
            actualTestConnectionStringSource.ActualConnectionString.InitialCatalog,

            credential: actualTestConnectionStringSource.ActualConnectionString.TryGetDbUserCredential());

        await actualTestConnectionStringSource.ActualConnectionString.ExecuteSqlFromFolderAsync("__Support/Scripts/Authorization", ct);
        await actualTestConnectionStringSource.ActualConnectionString.ExecuteSqlFromFolderAsync("__Support/Scripts/Configuration", ct);
        await actualTestConnectionStringSource.ActualConnectionString.ExecuteSqlFromFolderAsync("__Support/Scripts/SampleSystem", ct);

        new BssFluentMigrator(actualTestConnectionStringSource.ActualConnectionString.Value, typeof(InitNumberInDomainObjectEventMigration).Assembly).Migrate();
    }
}

