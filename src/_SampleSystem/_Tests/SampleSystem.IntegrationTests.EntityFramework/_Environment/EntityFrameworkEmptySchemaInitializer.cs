using Anch.Testing.Database.ConnectionStringManagement;

using Framework.AutomationCore.Extensions;
using Framework.Database.Domain;

using SampleSystem.DbGenerate.EntityFramework;

namespace SampleSystem.IntegrationTests._Environment;

public class EntityFrameworkEmptySchemaInitializer(IActualTestConnectionStringSource actualTestConnectionStringSource)
    : EmptySchemaInitializer(actualTestConnectionStringSource)
{
    public override async Task Initialize(CancellationToken ct)
    {
        var generator = new DbGeneratorTest();

        await generator.GenerateAllDb(
            actualTestConnectionStringSource.ActualConnectionString.DataSource,
            actualTestConnectionStringSource.ActualConnectionString.InitialCatalog,
            new DbUserCredential(
                actualTestConnectionStringSource.ActualConnectionString.UserId,
                actualTestConnectionStringSource.ActualConnectionString.Password),
            ct);


        await base.Initialize(ct);
    }
}
