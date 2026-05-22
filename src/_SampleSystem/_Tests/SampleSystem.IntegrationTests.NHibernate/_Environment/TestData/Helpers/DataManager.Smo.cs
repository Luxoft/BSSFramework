using Anch.Testing;

using Framework.AutomationCore.ServerManagement;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.SqlServer.Management.Smo;

namespace SampleSystem.IntegrationTests._Environment.TestData.Helpers;

public partial class DataManager
{
    public Table GetTable(string databaseName, string tableName, string schema = "app")
    {
        var database = this.RootServiceProvider.GetRequiredKeyedService<IServiceProvider>(ITestEnvironment.MainServiceProviderKey)
                           .GetRequiredService<ISqlServerFactory>().Create().TryGetDatabase().Databases[databaseName];

        return database.Tables[tableName, schema];
    }
}
