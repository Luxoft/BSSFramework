using Anch.Testing.Database;
using Anch.Testing.Database.ConnectionStringManagement;

using Microsoft.Data.SqlClient;

namespace Framework.AutomationCore.TestingProvider;

public class BssTestDatabaseConnectionStringBuilder(TestDatabaseSettings databaseSettings) : ITestDatabaseConnectionStringBuilder
{
    public TestDatabaseConnectionString AddPostfix(string postfix)
    {
        var builder = new SqlConnectionStringBuilder { ConnectionString = databaseSettings.DefaultConnectionString.Value };

        var initialCatalog = builder.InitialCatalog;

        if (string.IsNullOrWhiteSpace(initialCatalog))
            throw new InvalidOperationException("Initial Catalog is missing in connection string.");

        var directory = Path.GetDirectoryName(initialCatalog);
        var fileName = Path.GetFileNameWithoutExtension(initialCatalog);
        var extension = Path.GetExtension(initialCatalog);

        var newFileName = $"{fileName}{postfix}{extension}";
        var newDataSource = directory is null
                                ? newFileName
                                : Path.Combine(directory, newFileName);

        builder.InitialCatalog = newDataSource;

        return new TestDatabaseConnectionString(builder.ConnectionString);
    }
}
