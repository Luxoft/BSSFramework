using Anch.Testing.Database.ConnectionStringManagement;
using Anch.Testing.Database.Mssql.Extensions;

namespace Anch.Testing.Database.Mssql;

public class DatabaseCatalogResolver(
    MssqlDatabaseSettings settings,
    ITestConnectionStringPostfixFactory testConnectionStringPostfixFactory,
    ITestConnectionStringFactory testConnectionStringFactory) : IDatabaseCatalogResolver
{
    public IEnumerable<string> GetCatalogs(TestConnectionStringRole connectionStringRole)
    {
        var postfix = testConnectionStringPostfixFactory.Create(connectionStringRole);

        yield return testConnectionStringFactory.Create(postfix).GetInitialCatalog();

        foreach (var database in settings.SecondaryDatabases)
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
