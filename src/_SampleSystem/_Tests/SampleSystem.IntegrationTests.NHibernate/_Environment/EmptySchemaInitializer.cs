using Anch.Core;

using Framework.AutomationCore.Utils.DatabaseUtils;

//using Framework.AutomationCore.Utils.DatabaseUtils.Interfaces;
using Framework.Database.NHibernate.DBGenerator;

using SampleSystem.DbGenerate.NHibernate;

namespace SampleSystem.IntegrationTests._Environment;

public class EmptySchemaInitializer(IDatabaseContext databaseContext)
    : IInitializer
{
    public async Task Initialize(CancellationToken cancellationToken)
    {
        var generator = new DbGeneratorTest();

        generator.GenerateAllDB(
            databaseContext.ConnectionString.DataSource,
            databaseContext.ConnectionString.InitialCatalog,
            credential: DbUserCredential.Create(databaseContext.ConnectionString.UserId, databaseContext.ConnectionString.Password));
    }
}
