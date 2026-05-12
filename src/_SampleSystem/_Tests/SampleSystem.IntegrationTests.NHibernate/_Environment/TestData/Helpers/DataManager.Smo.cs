using Framework.AutomationCore.RootServiceProviderContainer;

using Microsoft.SqlServer.Management.Smo;

namespace SampleSystem.IntegrationTests._Environment.TestData.Helpers;

public partial class DataManager
{
    public Table GetTable(string databaseName, string tableName, string schema = "app")
    {
        var database = sqlServerSource.Server.Databases[databaseName];

        return database.Tables[tableName, schema];
    }
}
