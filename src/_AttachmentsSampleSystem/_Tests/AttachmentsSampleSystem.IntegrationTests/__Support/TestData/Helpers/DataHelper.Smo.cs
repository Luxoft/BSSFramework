using Microsoft.SqlServer.Management.Smo;

namespace AttachmentsSampleSystem.IntegrationTests.__Support.TestData.Helpers
{
    public partial class DataHelper
    {
        public Table GetTable(string serverName, string databaseName, string tableName, string schema = "app")
        {
            var server = new Server();
            server.ConnectionContext.ConnectionString = InitializeAndCleanup.DatabaseUtil.ConnectionSettings.ConnectionString;
            var database = server.Databases[databaseName];

            return database.Tables[tableName, schema];
        }
    }
}
