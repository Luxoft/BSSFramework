using Anch.Testing.Database.ConnectionStringManagement;

using Framework.AutomationCore.Extensions;

using MartinCostello.SqlLocalDb;

using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;

namespace Framework.AutomationCore.ServerManagement.LocalDb;

public class LocalDbSqlServerFactory(
    IActualTestConnectionStringSource actualTestConnectionStringSource,
    IOptions<AutomationFrameworkSettings> automationFrameworkSettingsOptions) : ISqlServerFactory
{
    private readonly string instanceName = ExtractInstanceName(actualTestConnectionStringSource.ActualConnectionString);

    private readonly string rootConnectionString =
        new SqlConnectionStringBuilder(actualTestConnectionStringSource.ActualConnectionString.Value) { InitialCatalog = "" }
            .ConnectionString;

    public ISqlServer Create()
    {
        EnsureInstanceStarted(this.instanceName);

        return new LocalDbSqlServer(
            new Server(new ServerConnection(new SqlConnection(this.rootConnectionString))),
            automationFrameworkSettingsOptions);
    }

    private static string ExtractInstanceName(TestConnectionString connectionString)
    {
        if (!connectionString.IsLocalDb)
        {
            throw new InvalidOperationException(
                $"Connection string data source '{connectionString.DataSource}' is not a LocalDB instance.");
        }

        return connectionString.DataSource.Split('\\', 2)[1];
    }

    private static void EnsureInstanceStarted(string instanceName)
    {
        using var localDb = new SqlLocalDbApi { AutomaticallyDeleteInstanceFiles = true };

        var instanceInfo = localDb.GetInstanceInfo(instanceName);

        if (!instanceInfo.Exists)
        {
            localDb.CreateInstance(instanceName);
        }

        if (!instanceInfo.IsRunning)
        {
            localDb.StartInstance(instanceName);
        }
    }
}
