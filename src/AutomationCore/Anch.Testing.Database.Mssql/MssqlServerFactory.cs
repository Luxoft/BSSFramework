using Anch.Testing.Database.ConnectionStringManagement;
using Anch.Testing.Database.Mssql.Extensions;

using MartinCostello.SqlLocalDb;

using Microsoft.Data.SqlClient;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;

namespace Anch.Testing.Database.Mssql;

public class MssqlServerFactory(
    IActualTestConnectionStringSource actualTestConnectionStringSource,
    TestDatabaseSettings testDatabaseSettings) : ISqlServerFactory, IDisposable
{
    private readonly string? localDbInstanceName = TryGetLocalDbInstanceName(actualTestConnectionStringSource.ActualConnectionString, testDatabaseSettings);

    public Server Create() =>
        new(
            new ServerConnection(
                new SqlConnection(
                    new SqlConnectionStringBuilder(actualTestConnectionStringSource.ActualConnectionString.Value) { InitialCatalog = "" }.ConnectionString)));

    private static string? TryGetLocalDbInstanceName(TestConnectionString connectionString, TestDatabaseSettings testDatabaseSettings)
    {
        if (connectionString.TryGetLocalDbInstanceName() is { } localDbInstanceName)
        {
            using var localDbApi = new SqlLocalDbApi();

            if (testDatabaseSettings.InitMode == DatabaseInitMode.RebuildSnapshot)
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
        if (this.localDbInstanceName is not null && testDatabaseSettings.InitMode == DatabaseInitMode.RebuildSnapshot)
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
