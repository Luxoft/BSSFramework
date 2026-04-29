using System.Data.Common;

using Anch.Testing.Database.ConnectionStringManagement;

namespace Anch.Testing.Database.Mssql;

public class MssqlTestDatabaseConnectionStringBuilder(TestDatabaseSettings databaseSettings) : ITestDatabaseConnectionStringBuilder
{
    public TestDatabaseConnectionString AddPostfix(string postfix)
    {
        var builder = new DbConnectionStringBuilder { ConnectionString = databaseSettings.DefaultConnectionString.Value };

        var dataSource = builder["Data Source"]?.ToString();

        if (string.IsNullOrWhiteSpace(dataSource))
            throw new InvalidOperationException("Data Source is missing in connection string.");

        var directory = Path.GetDirectoryName(dataSource);
        var fileName = Path.GetFileNameWithoutExtension(dataSource);
        var extension = Path.GetExtension(dataSource);

        var newFileName = $"{fileName}{postfix}{extension}";
        var newDataSource = directory is null
            ? newFileName
            : Path.Combine(directory, newFileName);

        builder["Data Source"] = newDataSource;

        return new TestDatabaseConnectionString(builder.ConnectionString);
    }
}
