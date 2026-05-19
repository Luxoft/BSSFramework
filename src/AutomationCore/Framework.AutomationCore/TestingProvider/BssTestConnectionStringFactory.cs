using Anch.Testing.Database;
using Anch.Testing.Database.ConnectionStringManagement;

using Microsoft.Data.SqlClient;

namespace Framework.AutomationCore.TestingProvider;

public class BssTestConnectionStringFactory(TestDatabaseSettings databaseSettings)
    : ITestConnectionStringFactory
{
    public TestConnectionString Create(string postfix)
    {
        var builder = new SqlConnectionStringBuilder { ConnectionString = databaseSettings.RawConnectionString.Value };

        builder.InitialCatalog += postfix;

        return new TestConnectionString(builder.ConnectionString);
    }
}
