using Microsoft.SqlServer.Management.Smo;

namespace Framework.AutomationCore.Extensions;

public static class SqlServerExtensions
{
    public static async Task DetachDatabaseAsync(this Server server, string databaseName, CancellationToken ct)
    {
        await server.SetModeRestrictedUserAsync(databaseName, ct);

        server.DetachDatabase(databaseName, false);
    }

    private static Table? GetTable(this Server server, string databaseName, string tableName)
    {
        var database = server.GetDatabase(databaseName);

        return database?.Tables[tableName];
    }

    public static long TableRowCount(this Server server, string databaseName, string tableName) => server.GetTable(databaseName, tableName)?.RowCount ?? 0;

    private static async Task SetModeRestrictedUserAsync(this Server server, string databaseName, CancellationToken ct)
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
            await server.ConnectionContext.SqlConnectionObject.ExecuteSqlAsync($"ALTER DATABASE [{databaseName}] SET RESTRICTED_USER WITH ROLLBACK IMMEDIATE", ct);
        }
    }

    private static async Task SetModeMultiUserAsync(this Server server, Microsoft.SqlServer.Management.Smo.Database database, CancellationToken ct)
        => await server.ConnectionContext.SqlConnectionObject.ExecuteSqlAsync($"ALTER DATABASE [{database.Name}] SET MULTI_USER", ct);

    public static Microsoft.SqlServer.Management.Smo.Database? GetDatabase(this Server server, string name) =>
        server.Databases.Contains(name) ? server.Databases[name] : null;
}

