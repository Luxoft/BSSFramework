using Anch.Core;
using Anch.Testing.Database;
using Anch.Testing.Database.ConnectionStringManagement;
using Anch.Testing.Database.Initializers;

using Microsoft.Extensions.DependencyInjection;

namespace Framework.AutomationCore.Services;

public class BssDatabaseSnapshotInitializer(
    [FromKeyedServices(BssEmptySchemaInitializer.Key)]
    IInitializer emptySchemaInitializer,
    [FromKeyedServices(TestDatabaseInitializer.TestDataKey)]
    IInitializer testDataInitializer,
    IDatabaseManager databaseManager,
    TestDatabaseSettings settings)
    : DatabaseSnapshotInitializer(emptySchemaInitializer, testDataInitializer, databaseManager, settings);

