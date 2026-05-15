using Anch.Testing.Database;
using Anch.Testing.Database.ConnectionStringManagement;

using Microsoft.Data.SqlClient;

namespace Framework.AutomationCore.TestingProvider;

public class BssTestConnectionStringFactory(TestDatabaseSettings databaseSettings, AutomationFrameworkSettings automationFrameworkSettings, DatabaseRandomizePostfix databaseRandomizePostfix)
    : ITestConnectionStringFactory
{
    public TestConnectionString Create(string postfix)
    {
        var builder = new SqlConnectionStringBuilder { ConnectionString = databaseSettings.RawConnectionString.Value };

        var baseInitialCatalog = builder.InitialCatalog;

        builder.InitialCatalog = baseInitialCatalog + postfix;

        if (automationFrameworkSettings.UseLocalDb)
        {
            builder.DataSource = $"(localdb)\\{baseInitialCatalog}_{databaseRandomizePostfix.Value}";
        }

        return new TestConnectionString(builder.ConnectionString);
    }
}

public class Loca
