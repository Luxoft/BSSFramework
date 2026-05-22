using Framework.AutomationCore.Services;
using MartinCostello.SqlLocalDb;

namespace Framework.AutomationCore.ServerManagement.LocalDb;

public class LocalDbSqlServerFactory : ISqlServerFactory
{
    public ISqlServer Create() => throw new NotImplementedException();
}

public class LocalDbSqlServer : ISqlServer
{
    public void Refresh()
    {
    }

    public ISqlServerDatabase? TryGetDatabase(string initialCatalog)
    {
        using var localDb = new SqlLocalDbApi();
        var instanceInfo = localDb.GetInstanceInfo(initialCatalog);
        localDb.AutomaticallyDeleteInstanceFiles = true;

        if (!instanceInfo.Exists)
        {
            localDb.CreateInstance(instanceInfo.Name);
        }

        if (!instanceInfo.IsRunning)
        {
            localDb.StartInstance(instanceInfo.Name);
        }
    }

    public ValueTask CreateEmptyAsync(string initialCatalog, DatabaseFileInfo fileInfo, CancellationToken ct) => throw new NotImplementedException();

    public ValueTask AttachDatabaseAsync(string initialCatalog, DatabaseFileInfo fileInfo, CancellationToken ct) => throw new NotImplementedException();

    public ValueTask DetachDatabaseAsync(string initialCatalog, CancellationToken ct) => throw new NotImplementedException();
}
