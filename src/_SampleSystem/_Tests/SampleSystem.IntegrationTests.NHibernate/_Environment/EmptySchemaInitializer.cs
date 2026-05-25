using Anch.Core;
using Anch.Testing.Database.ConnectionStringManagement;

using Framework.AutomationCore.Extensions;
using Framework.Database.NHibernate.DBGenerator;

using SampleSystem.DbGenerate.NHibernate;
using SampleSystem.IntegrationTests._Environment.FluentMigration;

namespace SampleSystem.IntegrationTests._Environment;

public class EmptySchemaInitializer(IActualTestConnectionStringSource actualTestConnectionStringSource) : IInitializer
{
    public async Task Initialize(CancellationToken cancellationToken)
    {
        var generator = new DbGeneratorTest();

        generator.GenerateAllDB(
            actualTestConnectionStringSource.ActualConnectionString.DataSource,
            actualTestConnectionStringSource.ActualConnectionString.InitialCatalog,

            credential: actualTestConnectionStringSource.ActualConnectionString.TryGetDbUserCredential());

        await actualTestConnectionStringSource.ActualConnectionString.ExecuteSqlFromFolderAsync("__Support/Scripts/Authorization", cancellationToken);
        await actualTestConnectionStringSource.ActualConnectionString.ExecuteSqlFromFolderAsync("__Support/Scripts/Configuration", cancellationToken);
        await actualTestConnectionStringSource.ActualConnectionString.ExecuteSqlFromFolderAsync("__Support/Scripts/SampleSystem", cancellationToken);

        new BssFluentMigrator(actualTestConnectionStringSource.ActualConnectionString.Value, typeof(InitNumberInDomainObjectEventMigration).Assembly).Migrate();
    }
}
