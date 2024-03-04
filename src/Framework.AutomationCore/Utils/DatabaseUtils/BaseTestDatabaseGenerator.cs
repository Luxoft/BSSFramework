using Automation.Utils.DatabaseUtils.Interfaces;
using Microsoft.SqlServer.Management.Smo;

namespace Automation.Utils.DatabaseUtils;

public abstract class BaseTestDatabaseGenerator
{
    public virtual IEnumerable<string> TestServers => Enumerable.Empty<string>();

    public IDatabaseContext DatabaseContext { get; }

    private readonly ConfigUtil configUtil;

    protected BaseTestDatabaseGenerator(IDatabaseContext databaseContext, ConfigUtil configUtil)
    {
        this.DatabaseContext = databaseContext;
        this.configUtil = configUtil;
    }

    public void CreateLocalDb()
    {
        if (this.configUtil.UseLocalDb && !CoreDatabaseUtil.LocalDbInstanceExists(this.DatabaseContext.Main.InstanceName))
        {
            CoreDatabaseUtil.CreateLocalDb(this.DatabaseContext.Main.InstanceName);
        }
    }

    public virtual void DeleteLocalDb()
    {
        if (this.configUtil.UseLocalDb)
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

    public void DeleteDetachedFiles() =>
        Directory.GetFiles(this.configUtil.DbDataDirectory)
            .Where(i => i.Contains(this.DatabaseContext.Main.InstanceName))
            .ToList()
            .ForEach(File.Delete);

    public virtual void CheckServerAllowed()
    {
        if (this.DatabaseContext.Server.NetName.Equals(this.configUtil.ComputerName, StringComparison.InvariantCultureIgnoreCase))
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
