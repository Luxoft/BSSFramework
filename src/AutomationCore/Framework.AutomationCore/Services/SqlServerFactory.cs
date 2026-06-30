using Anch.Testing.Database;
using Anch.Testing.Database.ConnectionStringManagement;

using Framework.AutomationCore.Extensions;

using MartinCostello.SqlLocalDb;

using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;

namespace Framework.AutomationCore.Services;

public class SqlServerFactory(
    IActualTestConnectionStringSource actualTestConnectionStringSource,
    IOptions<AutomationFrameworkSettings> automationFrameworkSettingsOptions) : ISqlServerFactory, IDisposable
{
    private readonly string? localDbInstanceName = TryGetLocalDbInstanceName(actualTestConnectionStringSource.ActualConnectionString, automationFrameworkSettingsOptions.Value);

    public Server Create() =>
        new(
            new ServerConnection(
                new SqlConnection(
                    new SqlConnectionStringBuilder(actualTestConnectionStringSource.ActualConnectionString.Value) { InitialCatalog = "" }.ConnectionString)));

    private static string? TryGetLocalDbInstanceName(TestConnectionString connectionString, AutomationFrameworkSettings settings)
    {
        if (connectionString.TryGetLocalDbInstanceName() is { } localDbInstanceName)
        {
            using var localDbApi = new SqlLocalDbApi();

            if (settings.DatabaseInitMode == DatabaseInitMode.RebuildSnapshot)
            {
                if (localDbApi.InstanceExists(localDbInstanceName))
                {
                    localDbApi.DeleteInstance(localDbInstanceName);
                }
            }

            localDbApi.CreateInstance(localDbInstanceName);

            return localDbInstanceName;
        }
        else
        {
            return null;
        }
    }

    public void Dispose()
    {
        if (this.localDbInstanceName is not null && automationFrameworkSettingsOptions.Value.DatabaseInitMode == DatabaseInitMode.RebuildSnapshot)
        {
            using var localDbApi = new SqlLocalDbApi();

            if (localDbApi.InstanceExists(this.localDbInstanceName))
            {
                localDbApi.StopInstance(this.localDbInstanceName);
                localDbApi.DeleteInstance(this.localDbInstanceName, true);
            }
        }
    }
}

