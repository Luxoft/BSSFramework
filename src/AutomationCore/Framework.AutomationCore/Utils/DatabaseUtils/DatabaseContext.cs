using System.Text.RegularExpressions;

using Automation.Settings;
using Automation.Utils.DatabaseUtils.Interfaces;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;
using SqlConnection = Microsoft.Data.SqlClient.SqlConnection;

namespace Automation.Utils.DatabaseUtils;

public class DatabaseContext : IDatabaseContext
{
    static DatabaseContext() =>
        LocalDbInstanceName = $"Test_{TextRandomizer.RandomString(10)}";

    private static readonly string LocalDbInstanceName;

    public DatabaseItem Main { get; }

    private readonly Server server;

    public Dictionary<string, DatabaseItem> Secondary { get; }

    public DatabaseContext(
        IConfiguration configuration,
        IOptions<AutomationFrameworkSettings> settings)
    : this(configuration, settings.Value)
    {
    }

    private DatabaseContext(
        IConfiguration configuration,
        AutomationFrameworkSettings settings)
    {
        var connectionString = this.GetConnectionString(configuration, settings);

        this.Main = new DatabaseItem(
            connectionString,
            settings.DatabaseCollation,
            settings.DbDataDirectory,
            null,
            settings.TestsParallelize);

        if (settings.SecondaryDatabases != null)
        {
            this.Secondary = new Dictionary<string, DatabaseItem>();
            foreach (var database in settings.SecondaryDatabases)
            {
                this.Secondary.Add(
                    database,
                    new DatabaseItem(
                        connectionString,
                        settings.DatabaseCollation,
                        settings.DbDataDirectory,
                        database,
                        settings.TestsParallelize));
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

    private string GetConnectionString(
        IConfiguration configuration,
        AutomationFrameworkSettings settings)
    {
        var connectionString = configuration.GetConnectionString(settings.ConnectionStringName);

        if (settings.UseLocalDb)
        {
            connectionString = this.GetLocalDbConnectionString(connectionString, LocalDbInstanceName);
        }

        return connectionString;
    }

    private string GetLocalDbConnectionString(string connectionString, string instanceName)
        => DataSourceRegex.Replace(connectionString, $"Data Source=(localdb)\\{instanceName}");

    private static readonly Regex DataSourceRegex = new Regex("Data Source=([^;]*)", RegexOptions.Compiled | RegexOptions.NonBacktracking);
}


