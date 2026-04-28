using System.Text.RegularExpressions;

using Microsoft.SqlServer.Management.Smo;

namespace Framework.AutomationCore.Utils.DatabaseUtils;

public static partial class CoreDatabaseUtil
{
    private static readonly Regex InitialCatalogRegex = new("Initial Catalog=(\\w+);", RegexOptions.Compiled);

    public static string CutInitialCatalog(string inputConnectionString) =>
        InitialCatalogRegex.Replace(inputConnectionString,"");

    private static string GetInitialCatalog(string connectionString) =>
        InitialCatalogRegex.Matches(connectionString).First().Value;

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
            ExecuteSql(server.ConnectionContext.SqlConnectionObject,$"ALTER DATABASE [{databaseName}] SET RESTRICTED_USER WITH ROLLBACK IMMEDIATE");
        }
    }

    private static void SetModeMultiUser(this Server server, Microsoft.SqlServer.Management.Smo.Database database)
        => ExecuteSql(server.ConnectionContext.SqlConnectionObject, $"ALTER DATABASE [{database.Name}] SET MULTI_USER");

    private static Microsoft.SqlServer.Management.Smo.Database? GetDatabase(this Server server, string name) => server.Databases.Contains(name) ? server.Databases[name] : null;
}
