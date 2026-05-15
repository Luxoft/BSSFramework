using Anch.Core;
using Anch.Testing.Database;
using Anch.Testing.Database.ConnectionStringManagement;
using Anch.Testing.Database.Initializers;

namespace Framework.AutomationCore.TestingProvider;

public class BssDatabaseSnapshotInitializer(
    IInitializer emptySchemaInitializer,
    IInitializer testDataInitializer,
    IDatabaseManager databaseManager,
    TestDatabaseSettings settings,
    ITestConnectionStringProvider connectionStringProvider)
    : DatabaseSnapshotInitializer(emptySchemaInitializer, testDataInitializer, databaseManager, settings, connectionStringProvider)
{
    protected override async Task InternalInitializeEmptySchema(CancellationToken cancellationToken)
    {


        await base.InternalInitializeEmptySchema(cancellationToken);
    }
}
