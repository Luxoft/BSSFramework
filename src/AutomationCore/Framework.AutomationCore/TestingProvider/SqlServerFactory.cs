using Anch.Testing.Database.ConnectionStringManagement;

using Microsoft.Data.SqlClient;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;

namespace Framework.AutomationCore.TestingProvider;

public class SqlServerFactory(IActualTestConnectionStringSource actualTestConnectionStringSource) : ISqlServerFactory
{
    public Server Create() =>
        new(
            new ServerConnection(
                new SqlConnection(
                    new SqlConnectionStringBuilder(actualTestConnectionStringSource.ActualConnectionString.Value) { InitialCatalog = "" }.ConnectionString)));
}
