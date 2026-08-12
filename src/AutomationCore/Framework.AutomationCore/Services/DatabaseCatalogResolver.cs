using Anch.Testing.Database.ConnectionStringManagement;

using Framework.AutomationCore.Extensions;

using Microsoft.Extensions.Options;

namespace Framework.AutomationCore.Services;

public class DatabaseCatalogResolver(
    IOptions<AutomationFrameworkSettings> automationFrameworkSettingsOptions,
    ITestConnectionStringPostfixFactory testConnectionStringPostfixFactory,
    ITestConnectionStringFactory testConnectionStringFactory) : IDatabaseCatalogResolver
{
    public IEnumerable<string> GetCatalogs(TestConnectionStringRole connectionStringRole)
    {
        var postfix = testConnectionStringPostfixFactory.Create(connectionStringRole);

        yield return testConnectionStringFactory.Create(postfix).InitialCatalog;

        foreach (var database in automationFrameworkSettingsOptions.Value.SecondaryDatabases)
        {
            if (string.IsNullOrWhiteSpace(postfix))
            {
                yield return database;
            }
            else
            {
                yield return database + "_" + postfix;
            }
        }
    }
}
