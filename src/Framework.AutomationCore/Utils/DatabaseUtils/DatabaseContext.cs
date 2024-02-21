using Automation.Utils.DatabaseUtils.Interfaces;

using Microsoft.Extensions.Options;
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
        IOptions<DatabaseContextSettings> settings)
    : this(configUtil, settings.Value)
    {
    }

    private DatabaseContext(
        ConfigUtil configUtil,
        DatabaseContextSettings settings)
    {
        var connectionString = configUtil.GetConnectionString(settings.ConnectionStringName);
        this.Main = new DatabaseItem(
            connectionString,
            configUtil.DatabaseCollation,
            configUtil.DbDataDirectory,
            null,
            configUtil.TestsParallelize);

        if (settings.SecondaryDatabases != null)
        {
            this.Secondary = new Dictionary<string, DatabaseItem>();
            foreach (var database in settings.SecondaryDatabases)
            {
                this.Secondary.Add(
                    database,
                    new DatabaseItem(
                        connectionString,
                        configUtil.DatabaseCollation,
                        configUtil.DbDataDirectory,
                        database,
                        configUtil.TestsParallelize));
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


