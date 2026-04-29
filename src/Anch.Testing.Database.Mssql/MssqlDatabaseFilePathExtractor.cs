using System.Data.Common;

using Anch.Testing.Database.ConnectionStringManagement;

namespace Anch.Testing.Database.Mssql;

public class MssqlDatabaseFilePathExtractor : IDatabaseFilePathExtractor
{
    public string Extract(TestDatabaseConnectionString connectionString)
    {
        var builder = new DbConnectionStringBuilder
        {
            ConnectionString = connectionString.Value
        };

        if (!builder.TryGetValue("Data Source", out var value))
            throw new InvalidOperationException("Data Source is missing.");

        var dataSource = value?.ToString();

        if (string.IsNullOrWhiteSpace(dataSource))
            throw new InvalidOperationException("Data Source is empty.");

        return dataSource;
    }
}
