using Anch.Core;
using Anch.Testing.Database.ConnectionStringManagement;

using Framework.AutomationCore;
using Framework.Database.NHibernate.DBGenerator;

using SampleSystem.DbGenerate.NHibernate;

namespace SampleSystem.IntegrationTests._Environment;

public class EmptySchemaInitializer(IActualTestConnectionStringSource actualTestConnectionStringSource) : IInitializer
{
    public async Task Initialize(CancellationToken cancellationToken)
    {
        var generator = new DbGeneratorTest();

        generator.GenerateAllDB(
            actualTestConnectionStringSource.ActualConnectionString.DataSource,
            actualTestConnectionStringSource.ActualConnectionString.InitialCatalog,
            credential: DbUserCredential.Create(
                actualTestConnectionStringSource.ActualConnectionString.UserId,
                actualTestConnectionStringSource.ActualConnectionString.Password));
    }
}
