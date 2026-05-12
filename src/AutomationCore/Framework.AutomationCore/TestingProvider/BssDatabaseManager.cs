using Anch.Testing.Database.ConnectionStringManagement;

using Framework.AutomationCore.Settings;

using Microsoft.Extensions.Options;

namespace Framework.AutomationCore.TestingProvider;

public class BssDatabaseManager(IMsSqlServerSource serverSource, IOptions<AutomationFrameworkSettings> settings) : IDatabaseManager
{
    public ValueTask<bool> Exists(TestDatabaseConnectionString connectionString, CancellationToken ct)
    {
        throw new NotImplementedException();
    }

    public ValueTask Remove(TestDatabaseConnectionString connectionString, CancellationToken ct) => throw new NotImplementedException();

    public ValueTask Copy(TestDatabaseConnectionString from, TestDatabaseConnectionString to, bool force, CancellationToken ct) => throw new NotImplementedException();

    public ValueTask Move(TestDatabaseConnectionString from, TestDatabaseConnectionString to, bool force, CancellationToken ct) => throw new NotImplementedException();
}
