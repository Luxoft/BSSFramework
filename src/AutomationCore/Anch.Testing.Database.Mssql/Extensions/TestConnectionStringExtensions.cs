using Anch.Testing.Database.ConnectionStringManagement;

using Microsoft.Data.SqlClient;

namespace Anch.Testing.Database.Mssql.Extensions;

public static class TestConnectionStringExtensions
{
    public static string GetUserId(this TestConnectionString connectionString) => connectionString.GetFromBuilder(v => v.UserID);

    public static string GetPassword(this TestConnectionString connectionString) => connectionString.GetFromBuilder(v => v.Password);

    public static string GetInitialCatalog(this TestConnectionString connectionString) => connectionString.GetFromBuilder(v => v.InitialCatalog);

    public static string GetDataSource(this TestConnectionString connectionString) => connectionString.GetFromBuilder(v => v.DataSource);

    public static string? TryGetLocalDbInstanceName(this TestConnectionString connectionString)
    {
        const string prefix = "(localdb)\\";
        var src = connectionString.GetDataSource();

        return src.StartsWith(prefix, StringComparison.OrdinalIgnoreCase)
                   ? src[prefix.Length..]
                   : null;
    }

    private static T GetFromBuilder<T>(this TestConnectionString connectionString, Func<SqlConnectionStringBuilder, T> selector) =>
        selector(new SqlConnectionStringBuilder(connectionString.Value));
}
