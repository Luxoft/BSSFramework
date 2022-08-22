using System.Collections.Generic;
using Automation.Utils.DatabaseUtils.Interfaces;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;
using SqlConnection = Microsoft.Data.SqlClient.SqlConnection;

namespace Automation.Utils.DatabaseUtils;

public class DatabaseContext : IDatabaseContext
{
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

        this.Server = new Server(new ServerConnection(new SqlConnection(
            CoreDatabaseUtil.CutInitialCatalog(this.Main.ConnectionString))));
    }

    public Dictionary<string, DatabaseItem> Secondary { get; }
    public Server Server { get; }
    public DatabaseItem Main { get; }
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