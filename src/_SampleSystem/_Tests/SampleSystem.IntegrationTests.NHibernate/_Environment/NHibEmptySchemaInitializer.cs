using Anch.Testing.Database.ConnectionStringManagement;

using Framework.AutomationCore.Extensions;

using SampleSystem.DbGenerate.NHibernate;

namespace SampleSystem.IntegrationTests._Environment;

public class NHibEmptySchemaInitializer(IActualTestConnectionStringSource actualTestConnectionStringSource)
    : EmptySchemaInitializer(actualTestConnectionStringSource)
{
    public override async Task Initialize(CancellationToken ct)
    {
        var generator = new DbGeneratorTest();

        generator.GenerateAllDb(
            actualTestConnectionStringSource.ActualConnectionString.DataSource,
            actualTestConnectionStringSource.ActualConnectionString.InitialCatalog,

            credential: actualTestConnectionStringSource.ActualConnectionString.TryGetDbUserCredential());

        await base.Initialize(ct);
    }
}
