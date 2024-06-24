using Automation.Settings;
using Automation.Utils.DatabaseUtils.Interfaces;

using Microsoft.Extensions.Options;
using Microsoft.SqlServer.Management.Smo;

namespace Automation.Utils.DatabaseUtils;

public abstract class BaseTestDatabaseGenerator
{
    public virtual IEnumerable<string> TestServers => Enumerable.Empty<string>();

    public IDatabaseContext DatabaseContext { get; }

    private readonly AutomationFrameworkSettings settings;

    protected BaseTestDatabaseGenerator(
        IDatabaseContext databaseContext,
        IOptions<AutomationFrameworkSettings> settings)
    {
        this.DatabaseContext = databaseContext;
        this.settings = settings.Value;
    }

    public void CreateLocalDb()
    {
        if (this.settings.UseLocalDb && !CoreDatabaseUtil.LocalDbInstanceExists(this.DatabaseContext.Main.InstanceName))
        {
            CoreDatabaseUtil.CreateLocalDb(this.DatabaseContext.Main.InstanceName);
        }
    }

    public virtual void DeleteLocalDb()
    {
        if (this.settings.UseLocalDb)
        {
            CoreDatabaseUtil.DeleteLocalDb(this.DatabaseContext.Main.InstanceName);
        }
    }

    public virtual void DropAllDatabases() =>
        this.DatabaseContext.Server.Databases.Cast<Database>()
            .Where(x => x.Name.Equals(this.DatabaseContext.Main.InitialCatalog))
            .ToList()
            .ForEach(x => x.Drop());

    public virtual void ExecuteInsertsForDatabases() =>
        CoreDatabaseUtil.ExecuteSqlFromFolder(
            this.DatabaseContext.Main.ConnectionString,
            @"__Support\Scripts",
            this.DatabaseContext.Main.DatabaseName);

    public void DeleteDetachedFiles()
    {
        if (!Directory.Exists(this.settings.DbDataDirectory))
        {
            return;
        }

        Directory.GetFiles(this.settings.DbDataDirectory)
                 .Where(i => i.Contains(this.DatabaseContext.Main.InstanceName))
                 .ToList()
                 .ForEach(File.Delete);
    }

    public virtual void CheckServerAllowed()
    {
        if (this.DatabaseContext.Server.NetName.Equals(Environment.MachineName, StringComparison.InvariantCultureIgnoreCase))
        {
            return;
        }

        if (!this.TestServers.Select(s => s.ToUpper())
                 .ToList()
                 .Contains(this.DatabaseContext.Server.NetName.ToUpper()))
        {
            throw new Exception(
                $"Server name {this.DatabaseContext.Server.NetName} is not specified in allowed list of test servers: {string.Join(", ", this.TestServers.Select(s => s.ToUpper()).ToList())}");
        }
    }
}
