using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.SqlServer.Management.Smo;

namespace WorkflowSampleSystem.IntegrationTests.__Support.TestData.Helpers
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
