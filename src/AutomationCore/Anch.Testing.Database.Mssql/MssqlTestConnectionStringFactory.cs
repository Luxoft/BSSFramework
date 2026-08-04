using Anch.Testing.Database.ConnectionStringManagement;

using Microsoft.Data.SqlClient;

namespace Anch.Testing.Database.Mssql;

public class MssqlTestConnectionStringFactory(TestDatabaseSettings databaseSettings) : ITestConnectionStringFactory
{
    public TestConnectionString Create(string postfix)
    {
        var builder = new SqlConnectionStringBuilder { ConnectionString = databaseSettings.RawConnectionString.Value };

        if (!string.IsNullOrWhiteSpace(postfix))
        {
            builder.InitialCatalog += "_" + postfix;
        }

        return new TestConnectionString(builder.ConnectionString);
    }
}
