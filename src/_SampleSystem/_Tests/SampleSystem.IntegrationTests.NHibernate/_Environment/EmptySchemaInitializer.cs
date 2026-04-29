using Anch.Core;

using Framework.AutomationCore.Utils.DatabaseUtils.Interfaces;
using Framework.Database.NHibernate.DBGenerator;

using SampleSystem.DbGenerate.NHibernate;

namespace SampleSystem.IntegrationTests._Environment;

public class EmptySchemaInitializer(IDatabaseContext databaseContext) : IInitializer
{
    public async Task Initialize(CancellationToken cancellationToken)
    {
        var generator = new DbGeneratorTest();

        // Act
        generator.GenerateAllDB(
            databaseContext.Main.DataSource,
            databaseContext.Main.DatabaseName,
            credential: DbUserCredential.Create(databaseContext.Main.UserId, databaseContext.Main.Password));
    }
}
