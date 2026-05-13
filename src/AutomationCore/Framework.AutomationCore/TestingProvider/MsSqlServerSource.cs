using Anch.Testing.Database;

using Microsoft.Data.SqlClient;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;

namespace Framework.AutomationCore.TestingProvider;

public class MsSqlServerSource(TestDatabaseSettings testDatabaseSettings) : IMsSqlServerSource
{
    public Server Server
    {
        get
        {
            field.Refresh();
            return field;
        }
    } = new(
        new ServerConnection(
            new SqlConnection(new SqlConnectionStringBuilder(testDatabaseSettings.MainConnectionString.Value) { InitialCatalog = "" }.ConnectionString)));
}
