using Automation.Utils.DatabaseUtils.Interfaces;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;
using SqlConnection = Microsoft.Data.SqlClient.SqlConnection;

namespace Automation.Utils.DatabaseUtils;

public class DatabaseContext : IDatabaseContext
{
    public DatabaseItem Main { get; }

    private readonly Server server;

    public Dictionary<string, DatabaseItem> Secondary { get; }

    public DatabaseContext(
        ConfigUtil configUtil,
        DatabaseContextSettings settings)
    {
        this.Main = new DatabaseItem(configUtil, settings.ConnectionString);

        if (settings.SecondaryDatabases != null)
        {
            this.Secondary = new Dictionary<string, DatabaseItem>();
            foreach (var database in settings.SecondaryDatabases)
            {
                this.Secondary.Add(database, new DatabaseItem(configUtil, settings.ConnectionString, database));
            }
        }

        this.server = new Server(new ServerConnection(new SqlConnection(
            CoreDatabaseUtil.CutInitialCatalog(this.Main.ConnectionString))));
    }

    public Server Server
    {
        get
        {
            this.server.Refresh();
            return this.server;
        }
    }
}

public class DatabaseContextSettings
{
    public string ConnectionString { get; set; }
    public string[] SecondaryDatabases { get; set; }

    public DatabaseContextSettings(string connectionString, string[] secondaryDatabases)
    {
        this.ConnectionString = connectionString;
        this.SecondaryDatabases = secondaryDatabases;
    }
}
