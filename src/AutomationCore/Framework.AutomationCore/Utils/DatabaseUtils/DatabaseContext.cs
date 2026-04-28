using System.Text.RegularExpressions;

using Framework.AutomationCore.Settings;
using Framework.AutomationCore.Utils.DatabaseUtils.Interfaces;
using Framework.Database;

using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;

namespace Framework.AutomationCore.Utils.DatabaseUtils;

public class DatabaseContext : IDatabaseContext
{
    static DatabaseContext() =>
        LocalDbInstanceName = $"Test_{TextRandomizer.RandomString(10)}";

    private static readonly string LocalDbInstanceName;

    public DatabaseItem Main { get; }

    public Dictionary<string, DatabaseItem> Secondary { get; }

    public DatabaseContext(
        IDefaultConnectionStringSource defaultConnectionStringSource,
        IOptions<AutomationFrameworkSettings> settings)
    : this(defaultConnectionStringSource, settings.Value)
    {
    }

    private DatabaseContext(IDefaultConnectionStringSource defaultConnectionStringSource, AutomationFrameworkSettings settings)
    {
        var actualConnectionString = GetConnectionString(defaultConnectionStringSource, settings);

        this.Main = new DatabaseItem(
            actualConnectionString,
            settings.DatabaseCollation,
            settings.DbDataDirectory,
            null,
            settings.TestsParallelize);

        {
            this.Secondary = new Dictionary<string, DatabaseItem>();
            foreach (var database in settings.SecondaryDatabases)
            {
                this.Secondary.Add(
                    database,
                    new DatabaseItem(
                        actualConnectionString,
                        settings.DatabaseCollation,
                        settings.DbDataDirectory,
                        database,
                        settings.TestsParallelize));
            }
        }

        this.Server = new Server(new ServerConnection(new SqlConnection(
            CoreDatabaseUtil.CutInitialCatalog(this.Main.ConnectionString))));
    }

    public Server Server
    {
        get
        {
            field.Refresh();
            return field;
        }
    }

    private static string GetConnectionString(
        IDefaultConnectionStringSource defaultConnectionStringSource,
        AutomationFrameworkSettings settings)
    {
        if (settings.UseLocalDb)
        {
            return GetLocalDbConnectionString(defaultConnectionStringSource.ConnectionString, LocalDbInstanceName);
        }

        return defaultConnectionStringSource.ConnectionString;
    }

    private static string GetLocalDbConnectionString(string connectionString, string instanceName)
        => DataSourceRegex.Replace(connectionString, $"Data Source=(localdb)\\{instanceName}");

    private static readonly Regex DataSourceRegex = new("Data Source=([^;]*)", RegexOptions.Compiled | RegexOptions.NonBacktracking);
}


