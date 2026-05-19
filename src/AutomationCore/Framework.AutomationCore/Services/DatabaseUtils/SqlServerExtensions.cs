using Microsoft.SqlServer.Management.Smo;

namespace Framework.AutomationCore.Services.DatabaseUtils;

public static class SqlServerExtensions
{
    public static void DetachDatabase(this Server server, string databaseName)
    {
        server.SetModeRestrictedUser(databaseName);

        server.DetachDatabase(databaseName, false);
    }

    private static Table? GetTable(this Server server, string databaseName, string tableName)
    {
        var database = server.GetDatabase(databaseName);

        return database?.Tables[tableName];
    }

    public static long TableRowCount(this Server server, string databaseName, string tableName) => server.GetTable(databaseName, tableName)?.RowCount ?? 0;

    private static void SetModeRestrictedUser(this Server server, string databaseName)
    {
        if (server.GetDatabase(databaseName) == null)
        {
            return;
        }

        try
        {
            server.KillAllProcesses(databaseName);
        }
        catch (FailedOperationException)
        {
            server.ConnectionContext.SqlConnectionObject.ExecuteSql($"ALTER DATABASE [{databaseName}] SET RESTRICTED_USER WITH ROLLBACK IMMEDIATE");
        }
    }

    private static void SetModeMultiUser(this Server server, Microsoft.SqlServer.Management.Smo.Database database)
        => server.ConnectionContext.SqlConnectionObject.ExecuteSql($"ALTER DATABASE [{database.Name}] SET MULTI_USER");

    public static Microsoft.SqlServer.Management.Smo.Database? GetDatabase(this Server server, string name) =>
        server.Databases.Contains(name) ? server.Databases[name] : null;
}
