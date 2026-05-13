using Anch.Testing.Database.ConnectionStringManagement;

using Microsoft.Data.SqlClient;

namespace Framework.AutomationCore.Utils.DatabaseUtils;

public static class TestDatabaseConnectionStringExtensions
{
    extension(TestConnectionString connectionString)
    {
        public string UserId => connectionString.GetFromBuilder(v => v.UserID);

        public string Password => connectionString.GetFromBuilder(v => v.Password);

        public string InitialCatalog => connectionString.GetFromBuilder(v => v.InitialCatalog);

        public string DataSource => connectionString.GetFromBuilder(v => v.DataSource);

        private T GetFromBuilder<T>(Func<SqlConnectionStringBuilder, T> selector) => selector(connectionString.GetSqlConnectionStringBuilder());

        private SqlConnectionStringBuilder GetSqlConnectionStringBuilder() => new(connectionString.Value);
    }
}
