using System.Collections.Generic;
using Automation.Utils.DatabaseUtils.Interfaces;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;
using SqlConnection = Microsoft.Data.SqlClient.SqlConnection;

namespace Automation.Utils.DatabaseUtils;

public class DatabaseContext : IDatabaseContext
{
    public DatabaseContext(
        string connectionString,
        string[] secondaryDatabases = null)
    {
        this.MainDatabase = new DatabaseItem(connectionString);

        if (secondaryDatabases != null)
        {
            this.SecondaryDatabases = new Dictionary<string, DatabaseItem>();
            foreach (var database in secondaryDatabases)
            {
                this.SecondaryDatabases.Add(database, new DatabaseItem(connectionString, database));
            }
        }

        this.Server = new Server(new ServerConnection(new SqlConnection(
            CoreDatabaseUtil.CutInitialCatalog(this.MainDatabase.ConnectionString))));

        if (ConfigUtil.UseLocalDb && !CoreDatabaseUtil.LocalDbInstanceExists(this.MainDatabase.InstanceName))
        {
            CoreDatabaseUtil.CreateLocalDb(this.MainDatabase.InstanceName);
        }
    }

    public Dictionary<string, DatabaseItem> SecondaryDatabases { get; }

    public Server Server { get; }
    public DatabaseItem MainDatabase { get; }

    public void Dispose()
    {
        if (ConfigUtil.UseLocalDb)
        {
            CoreDatabaseUtil.DeleteLocalDb(this.MainDatabase.InstanceName);
        }
    }
}